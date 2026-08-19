using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Maneuvers;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop.Input;

public sealed class InputHandler
{
    private readonly ThrusterSystem
    _thrusterSystem = new();

    public void Handle(
    SimulationState state,
    Universe universe)
    {
        HandleCameraTarget(
            state,
            universe);

        HandleOrbitCamera(state);

        HandleSimulationSpeed(state);

        HandlePause(state);

        HandleThrusters(state);

        FullScreen();

        HandleControlledBody(state, universe);

        TransferPlanet(
            state,
            universe);
    }

    public void FullScreen()
    {
        if(IsKeyPressed(KeyboardKey.One))
            ToggleFullscreen();

        if (IsKeyPressed(KeyboardKey.Two))
            ToggleFullscreen();
    }

    private void HandleThrusters(
    SimulationState state)
    {
        if (state.ControlledBody is null)
        {
            return;
        }

        var body =
            state.ControlledBody;

        if (body.Type != BodyType.Spacecraft)
        {
            return;
        }

        const double thrust = 50;

        const double dt = 0.001;

        if (IsKeyDown(
            KeyboardKey.B))
        {
            _thrusterSystem
                .ApplyProgradeThrust(
                    body,
                    thrust,
                    dt);
        }

        if (IsKeyDown(
            KeyboardKey.N))
        {
            _thrusterSystem
                .ApplyRetrogradeThrust(
                    body,
                    thrust,
                    dt);
        }
    }

    private void HandleControlledBody(
    SimulationState state,
    Universe universe)
    {
        if (!IsKeyPressed(
            KeyboardKey.C))
        {
            return;
        }

        var spacecrafts =
            universe.Bodies
                .Where(x =>
                    x.Type ==
                    BodyType.Spacecraft)
                .ToList();

        if (spacecrafts.Count == 0)
        {
            return;
        }

        var current =
            spacecrafts.IndexOf(
                state.ControlledBody!);

        current++;

        if (current >= spacecrafts.Count)
        {
            current = 0;
        }

        state.ControlledBody =
            spacecrafts[current];
    }
    // TODO:
    // Temporary heliocentric Hohmann transfer experiment.
    //
    // Current scientific contract:
    // - Maneuvering body: controlled spacecraft
    // - Central body: Sun
    // - Reference: relative spacecraft-to-Sun position
    // - Target orbit: 1.5 times the current orbital radius
    //
    // This will eventually be replaced by a proper Mission Planner.
    private void TransferPlanet(
        SimulationState state,
        Universe universe)
    {
        if (!IsKeyPressed(
            KeyboardKey.H))
        {
            return;
        }

        // The camera target is a presentation concept.
        // The controlled spacecraft is the body that actually performs
        // the orbital maneuver.
        var spacecraft =
            state.ControlledBody;

        if (spacecraft is null ||
            spacecraft.Type != BodyType.Spacecraft)
        {
            return;
        }

        // The current experimental Hohmann model is heliocentric,
        // so the Sun is explicitly treated as the central body.
        var centralBody =
            universe.FindBody("Sun");

        if (centralBody is null)
        {
            return;
        }

        // Orbital radius is a relative distance:
        //
        //     r⃗ = x⃗_spacecraft - x⃗_centralBody
        //     r  = |r⃗|
        //
        // Using spacecraft.Position.Magnitude() would instead measure
        // distance from the global coordinate origin.
        var relativePosition =
            spacecraft.Position -
            centralBody.Position;

        var currentRadius =
            relativePosition.Magnitude();

        if (!double.IsFinite(currentRadius) ||
            currentRadius <= 0)
        {
            return;
        }

        // Temporary experiment:
        // raise the target circular orbit to 1.5 times
        // the current heliocentric orbital radius.
        var targetRadius =
            currentRadius * 1.5;

        var planner =
            new ManeuverPlanner();
        
        // The maneuver is defined in a body-centered frame.
        // For the current experiment, the Sun is the central body.
        var referenceContext =
            new ReferenceContext(
                ReferenceFrame.BodyCentered,
                centralBody);

        var plan =
            planner.PlanTransfer(
                currentRadius,
                targetRadius,
                referenceContext);

        state.CurrentPlan =
            plan;

        // Important:
        // the spacecraft receives the maneuver,
        // not whichever object the camera happens to observe.
        state.BurnTarget =
            spacecraft;

        state.BurnState.BurnStep = 0;
        state.BurnState.BurnTimer = 0;

        state.BurnExecuted =
            true;
    }


    private void HandleCameraTarget(
    SimulationState state,
    Universe universe)
    {
        var bodies =
            universe.Bodies.ToList();
        

        if (IsKeyPressed(KeyboardKey.R))
        {
            state.Camera.Target =
                universe.FindBody("Sun");
        }

        if (IsKeyPressed(KeyboardKey.Right))
        {
            state.SelectedBodyIndex++;

            if (state.SelectedBodyIndex >= bodies.Count)
            {
                state.SelectedBodyIndex = 0;
            }

            state.Camera.Target =
                bodies[state.SelectedBodyIndex];
        }

        if (IsKeyPressed(KeyboardKey.Left))
        {
            state.SelectedBodyIndex--;

            if (state.SelectedBodyIndex < 0)
            {
                state.SelectedBodyIndex =
                    bodies.Count - 1;
            }

            state.Camera.Target =
                bodies[state.SelectedBodyIndex];
        }

    }

    private void HandleOrbitCamera(
        SimulationState state)
    {
        var wheel = GetMouseWheelMove();

        state.Camera.Distance -= wheel * 20;

        state.Camera.Distance =
            Math.Clamp(
                state.Camera.Distance,
                50,
                2000);

        if (IsMouseButtonDown(MouseButton.Right))
        {
            var delta = GetMouseDelta();

            state.Camera.AngleX += delta.X * 0.01;
            state.Camera.AngleY += delta.Y * 0.01;

            state.Camera.AngleY =
                Math.Clamp(
                    state.Camera.AngleY,
                    -1.5,
                    1.5);
        }
    }

    private void HandleSimulationSpeed(
        SimulationState state)
    {
        if (IsKeyPressed(KeyboardKey.Up))
        {
            state.SimulationSpeed += 10;
        }

        if (IsKeyPressed(KeyboardKey.Down))
        {
            state.SimulationSpeed -= 10;
        }

        state.SimulationSpeed =
            Math.Max(
                1,
                state.SimulationSpeed);
    }

    private void HandlePause(
        SimulationState state)
    {
        if (IsKeyPressed(KeyboardKey.Space))
        {
            state.Paused =
                !state.Paused;
        }
    }
}