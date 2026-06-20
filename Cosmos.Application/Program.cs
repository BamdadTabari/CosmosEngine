using Cosmos.Domain.Entities;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;

var universe = new Universe();

var body = new Body(
    new Position(0, 0),
    new Velocity(0, 0),
    new Acceleration(2, 0),
    new Mass(1));

var body2 = new Body(
    new Position(100, 0),
    new Velocity(0, 0),
    new Acceleration(2, 0),
    new Mass(2));

universe.AddBody(body);
universe.AddBody(body2);

IPhysicsModel physics =
    new NewtonianPhysicsModel();

for (int i = 0; i < 10; i++)
{
    physics.Step(universe, 1);

    Console.WriteLine(
        $"Position b1: {body.Position.X}");

    Console.WriteLine(
        $"Velocity b1: {body.Velocity.X}");

    Console.WriteLine(
        $"Position b2: {body2.Position.X}");

    Console.WriteLine(
        $"Velocity b2: {body2.Velocity.X}");

    Console.WriteLine();
}

