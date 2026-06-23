using Cosmos.Domain.Entities;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class InputHandler
{
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
    }

    private void HandleCameraTarget(
    SimulationState state,
    Universe universe)
    {
        var bodies =
            universe.Bodies.ToList();

        if (IsKeyPressed(KeyboardKey.One)
            && bodies.Count > 0)
        {
            state.Camera.Target = bodies[0];
        }

        if (IsKeyPressed(KeyboardKey.Two)
            && bodies.Count > 1)
        {
            state.Camera.Target = bodies[1];
        }

        if (IsKeyPressed(KeyboardKey.Three)
            && bodies.Count > 2)
        {
            state.Camera.Target = bodies[2];
        }

        if (IsKeyPressed(KeyboardKey.Four)
            && bodies.Count > 3)
        {
            state.Camera.Target = bodies[3];
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