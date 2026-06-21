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

    camera.Target = giant;

    for (int i = 0; i < 100; i++)
    {
        physics.Step(universe, 0.001);
    }

    BeginDrawing();

    ClearBackground(Color.Black);

    foreach (var body in universe.Bodies)
    {

        if (!Trails.ContainsKey(body.Id))
        {
            Trails[body.Id] = new Queue<Vector2D>();
        }

        var trail = Trails[body.Id];

        trail.Enqueue(body.Position);

        while (trail.Count > 300)
        {
            trail.Dequeue();
        }

        foreach (var point in trail)
        {
            DrawCircle(
                centerX +
                    (int)(point.X),
                centerY +
                    (int)(point.Y),
                1,
                Color.DarkGray);
        }

        if(body.Mass.Value >10000)
        {
            DrawCircle(
            centerX +
                    (int)(body.Position.X - camera.Position.X),
                centerY +
                    (int)(body.Position.Y - camera.Position.Y),
            15,
            Color.White);
        }
        else
        {
            DrawCircle(
            centerX +
                    (int)(body.Position.X - camera.Position.X),
                centerY +
                    (int)(body.Position.Y - camera.Position.Y),
            5,
            Color.White);
        }
    }

    EndDrawing();
}

CloseWindow();