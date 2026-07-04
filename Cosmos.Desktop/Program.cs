using Cosmos.Desktop;
using Cosmos.Desktop.Input;
using Cosmos.Desktop.Loaders;
using Cosmos.Desktop.Rendering;
using Cosmos.Desktop.Styles;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Calculators;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Integrators;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Services;
using Cosmos.Engine.Tracking;
using Raylib_cs;
using static Raylib_cs.Raylib;

// TODO:
// HI 
// Im not insane
// all of these will be refactored

IIntegrator integrator =
    new SemiImplicitEulerIntegrator();

IPhysicsModel physics =
    new NewtonianPhysicsModel(
        integrator);

var orbitalTracker =
    new OrbitalTracker();

var state =
    new SimulationState();

var inputHandler =
    new InputHandler();

var styleLoader =
    new StyleLoader();

var loader =
    new UniverseLoader();

var executionSystem =
    new ManeuverExecutionSystem();

var universe =
    loader.Load(
        "DataFiles/solar-system.json");

var styleConfig =
    styleLoader.Load(
        "DataFiles/body-styles.json");

var renderer =
    new UniverseRenderer(
        new PlanetStyleProvider(
            styleConfig));

var hudRenderer =
    new HudRenderer();

Dictionary<Guid, Queue<Vector3D>>
    Trails = [];

InitWindow(
    1280,
    720,
    "Cosmos Engine");

SetTargetFPS(60);


var sun =
universe
    .FindBody("Sun");

var earth = universe.FindBody("Earth");

var orbitCalculator =
    new CircularOrbitCalculator();

var shipPosition =
    earth.Position +
    new Vector3D(
        15,
        0,
        0);

var radius =
    (shipPosition - sun.Position)
    .Magnitude();

var orbitalVelocity =
    orbitCalculator.Calculate(
        sun.Mass.Value,
        radius);

var tangent =
    new Vector3D(
        0,
        1,
        0);

var ship =
    new Spacecraft(
        shipPosition,

        tangent *
        orbitalVelocity,

        new Mass(0.01),

        "Explorer-1");

universe.AddBody(ship);

state.Camera.Target = sun;

var explorer =
    universe.FindBody(
        "Explorer-1");

state.ControlledBody =
    explorer;

while (!WindowShouldClose())
{
    inputHandler.Handle(
        state,
        universe);


    if (!state.Paused)
    {
        var burnTarget = state.BurnTarget;

        for (int i = 0; i < state.SimulationSpeed; i++)
        {
            physics.Step(universe, 0.001);
            orbitalTracker.Update(universe.Bodies);
        }

        if (burnTarget is not null)
        {

            executionSystem.Update(
            burnTarget,
            state.CurrentPlan,
            0.001,
            state.BurnState);
        }
    }

    renderer.Render(
        universe,
        state.Camera,
        Trails);

    hudRenderer.Render(
        universe,
        state.Camera,
        state.SimulationSpeed,
        orbitalTracker,
        state);
}
CloseWindow();