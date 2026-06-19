using Cosmos.Domain.Entities;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;

var universe = new Universe();

var body = new Body(
    new Position(0, 0),
    new Velocity(10, 0),
    new Mass(1));

universe.AddBody(body);

IPhysicsModel physics =
    new NewtonianPhysicsModel();

physics.Step(universe, 1);

Console.WriteLine(
    $"X={body.Position.X}, Y={body.Position.Y}");

for (int i = 0; i < 10; i++)
{
    physics.Step(universe, i);

    Console.WriteLine(
        body.Position.X);
}

