using Cosmos.Desktop;
using Cosmos.Desktop.Input;
using Cosmos.Desktop.Loaders;
using Cosmos.Desktop.Rendering;
using Cosmos.Desktop.Styles;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Integrators;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Services;
using Cosmos.Engine.Tracking;
using Raylib_cs;
using static Raylib_cs.Raylib;


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

InitWindow(1280, 720, "Cosmos Engine");

SetTargetFPS(60);


var sun =
universe
    .FindBody("Sun");

state.Camera.Target = sun;


while (!WindowShouldClose())
{
    inputHandler.Handle(
        state,
        universe);

    if (!state.Paused)
    {
        for (int i = 0; i < state.SimulationSpeed; i++)
        {
            physics.Step(
                universe,
                0.001);

            orbitalTracker.Update(
                universe.Bodies);
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