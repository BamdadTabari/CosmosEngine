using Cosmos.Desktop;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;
using Raylib_cs;
using static Raylib_cs.Raylib;


IIntegrator integrator =
    new SemiImplicitEulerIntegrator();

IPhysicsModel physics =
    new NewtonianPhysicsModel(
        integrator);

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
        "Data/solar-system.json");

var styleConfig =
    styleLoader.Load(
        "Data/body-styles.json");

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

    for (int i = 0; i < 100; i++)
    {
        physics.Step(universe, 0.001);
    }

    inputHandler.Handle(state,universe);

    renderer.Render(
    universe,
    state.Camera,
    Trails);

    hudRenderer.Render(
    universe,
    state.Camera,
    state.SimulationSpeed);

}
CloseWindow();