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

var camera = new Camera();

int simulationSpeed = 100;

bool paused = false;

var renderer =
    new UniverseRenderer();

var universe = new Universe();

var giant = new Body(
    new Vector2D(0, 0),
    new Vector2D(0, 0),
    new Mass(100000));

var small1 = new Body(
    new Vector2D(80, 0),
    new Vector2D(0, 350),
    new Mass(10));

var small2 = new Body(
    new Vector2D(140, 0),
    new Vector2D(0, 270),
    new Mass(10));

var small3 = new Body(
    new Vector2D(220, 0),
    new Vector2D(0, 210),
    new Mass(10));

universe.AddBody(giant);
universe.AddBody(small1);
universe.AddBody(small2);
universe.AddBody(small3);

Dictionary<Guid, Queue<Vector2D>>
    Trails = [];

InitWindow(1280, 720, "Cosmos Engine");

SetTargetFPS(60);

const int centerX = 640;
const int centerY = 360;


while (!WindowShouldClose())
{

    if (camera.Target is not null)
    {
        camera.Position =
            camera.Target.Position;
    }

    if (IsKeyPressed(KeyboardKey.One))
    {
        camera.Target = giant;
    }

    if (IsKeyPressed(KeyboardKey.Two))
    {
        camera.Target = small1;
    }

    if (IsKeyPressed(KeyboardKey.Three))
    {
        camera.Target = small2;
    }

    if (IsKeyPressed(KeyboardKey.Four))
    {
        camera.Target = small3;
    }

    if (IsKeyDown(KeyboardKey.Equal))
    {
        camera.Zoom += 0.01;
    }

    if (IsKeyDown(KeyboardKey.Minus))
    {
        camera.Zoom -= 0.01;
    }

    camera.Zoom =
    Math.Max(0.1, camera.Zoom);

    if (IsKeyPressed(KeyboardKey.Up))
    {
        simulationSpeed += 10;
    }

    if (IsKeyPressed(KeyboardKey.Down))
    {
        simulationSpeed -= 10;
    }

    simulationSpeed =
    Math.Max(1, simulationSpeed);


    if (IsKeyPressed(KeyboardKey.Space))
    {
        paused = !paused;
    }

    if (!paused)
    {
        for (int i = 0; i < simulationSpeed; i++)
        {
            physics.Step(universe, 0.001);
        }
    }

    BeginDrawing();

    ClearBackground(Color.Black);

    foreach (var body in universe.Bodies)
    {

        BeginDrawing();

        ClearBackground(Color.Black);

        renderer.Render(
            universe,
            camera,
            Trails);

        EndDrawing();
    }
}
CloseWindow();