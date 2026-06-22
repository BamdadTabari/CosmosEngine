using Cosmos.Desktop;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;
using Raylib_cs;
using static Raylib_cs.Raylib;


IPhysicsModel physics =
    new NewtonianPhysicsModel();

var state =
    new SimulationState();

var inputHandler =
    new InputHandler();


var renderer =
    new UniverseRenderer();

var hudRenderer =
    new HudRenderer();

var universe = new Universe();

var giant = new Body(
    new Vector3D (0, 0,0),
    new Vector3D (0, 0, 0),
    new Mass(100000),
    "giant");

var small1 = new Body(
    new Vector3D (80, 0, 0),
    new Vector3D (0, 350, 0),
    new Mass(10),
    "small1");

var small2 = new Body(
    new Vector3D (140, 0, 120),
    new Vector3D (0, 270, 0),
    new Mass(10),
    "small2");

var small3 = new Body(
    new Vector3D (220, 0, 0),
    new Vector3D (0, 210, 0),
    new Mass(10),
    "small3");

universe.AddBody(giant);
universe.AddBody(small1);
universe.AddBody(small2);
universe.AddBody(small3);

Dictionary<Guid, Queue<Vector3D>>
    Trails = [];

InitWindow(1280, 720, "Cosmos Engine");

SetTargetFPS(60);


while (!WindowShouldClose())
{

    inputHandler.Handle(
    state,
    giant,
    small1,
    small2,
    small3);

    if (state.Camera.Target is not null)
    {
        state.Camera.Position =
            state.Camera.Target.Position;
    }

    if (!state.Paused)
    {
        for (int i = 0; i < state.SimulationSpeed; i++)
        {
            physics.Step(universe, 0.001);
        }
    }


    renderer.Render(
    universe,
    state.Camera,
    Trails);

    hudRenderer.Render(
    state.Camera,
    state.SimulationSpeed,
    state.Paused);

}
CloseWindow();