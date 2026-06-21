using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;
using Raylib_cs;
using static Raylib_cs.Raylib;


IPhysicsModel physics =
    new NewtonianPhysicsModel();

var universe = new Universe();

universe.AddBody(
    new Body(
        new Vector2D(200, 100),
        new Vector2D(0, 0),
        new Mass(15)));

universe.AddBody(
    new Body(
        new Vector2D(400, 200),
        new Vector2D(0, 0),
        new Mass(25)));

universe.AddBody(
    new Body(
        new Vector2D(600, 300),
        new Vector2D(0, 0),
        new Mass(35)));

InitWindow(1280, 720, "Cosmos Engine");

SetTargetFPS(60);

while (!WindowShouldClose())
{
    physics.Step(universe, 0.01);

    BeginDrawing();

    ClearBackground(Color.Black);

    foreach (var body in universe.Bodies)
    {
        DrawBody(body);
    }

    EndDrawing();
}

CloseWindow();

//////////////////////
///
void DrawBody(Body body)
{
    const int centerX = 640;
    const int centerY = 360;

    DrawCircle(
        centerX + (int)body.Position.X,
        centerY + (int)body.Position.Y,
        5,
        Color.White);
}