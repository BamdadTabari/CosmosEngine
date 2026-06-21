using Cosmos.Application.Renderers;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;

Console.CursorVisible = false;

var universe = new Universe();

var giant = new Body(
    new Vector2D(0, 0),
    new Vector2D(0, 0),
    new Mass(100000));

var small1 = new Body(
    new Vector2D(40, 0),
    new Vector2D(0, 20),
    new Mass(10));

var small2 = new Body(
    new Vector2D(50, 0),
    new Vector2D(0, 23),
    new Mass(10));

var small3 = new Body(
    new Vector2D(60, 0),
    new Vector2D(0, 17),
    new Mass(10));


universe.AddBody(giant);
universe.AddBody(small1);
universe.AddBody(small2);
universe.AddBody(small3);

IPhysicsModel physics =
    new NewtonianPhysicsModel();

var renderer =
    new ConsoleUniverseRenderer();

while (true)
{
    physics.Step(universe, 0.001);

    renderer.Render(universe);

    Thread.Sleep(10);
}