using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;
using System.Numerics;

var universe = new Universe();

var body = new Body(
    new Vector2D(36, 3),
    new Vector2D(0, 0),
    new Mass(1));

var body2 = new Body(
    new Vector2D(126, 47),
    new Vector2D(0, 0),
    new Mass(2));

var body3 = new Body(
    new Vector2D(29, 19),
    new Vector2D(0, 0),
    new Mass(20));

universe.AddBody(body);
universe.AddBody(body2);
universe.AddBody(body3);

IPhysicsModel physics =
    new NewtonianPhysicsModel();

for (int i = 0; i < 100; i++)
{
    physics.Step(universe, 0.1);

    Console.WriteLine(
        $"Position b1=> X: {body.Position.X}, Y: {body.Position.Y}");

    Console.WriteLine(
        $"Velocity b1=> X: {body.Velocity.X}, Y: {body.Velocity.Y}");

    Console.WriteLine(
    $"Position b2=> X: {body2.Position.X}, Y: {body2.Position.Y}");

    Console.WriteLine(
    $"Velocity b2=> X: {body2.Velocity.X}, Y: {body2.Velocity.Y}");

    Console.WriteLine(
    $"Position b3=> X: {body3.Position.X}, Y: {body3.Position.Y}");

    Console.WriteLine(
    $"Velocity b3=> X: {body3.Velocity.X}, Y: {body3.Velocity.Y}");

    Console.WriteLine();
}

