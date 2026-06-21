using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;

// TODO:
// Temporary simulation playground.
//
// This file currently contains:
// - Scenario creation
// - User input
// - Diagnostics
// - Console rendering
// - Statistics
//
// These concerns should eventually be moved into
// dedicated components/services.
//
// Program.cs is intentionally being used as a sandbox
// while experimenting with physics models.

var universe = new Universe();

Console.Write("How many bodies do you want to create? ");

var bodyCount =
    int.Parse(Console.ReadLine()!);

for (int i = 1; i <= bodyCount; i++)
{
    Console.WriteLine();
    Console.WriteLine($"=== Body {i} ===");

    Console.Write("Position X: ");
    var posX = double.Parse(Console.ReadLine()!);

    Console.Write("Position Y: ");
    var posY = double.Parse(Console.ReadLine()!);

    Console.Write("Velocity X: ");
    var velX = double.Parse(Console.ReadLine()!);

    Console.Write("Velocity Y: ");
    var velY = double.Parse(Console.ReadLine()!);

    Console.Write("Mass: ");
    var mass = double.Parse(Console.ReadLine()!);

    var body = new Body(
        new Vector2D(posX, posY),
        new Vector2D(velX, velY),
        new Mass(mass));

    universe.AddBody(body);
}

Console.WriteLine();

Console.Write("Delta Time: ");
var deltaTime =
    double.Parse(Console.ReadLine()!);

Console.Write("Number Of Steps: ");
var stepCount =
    int.Parse(Console.ReadLine()!);

IPhysicsModel physics =
    new NewtonianPhysicsModel();

for (int step = 1; step <= stepCount; step++)
{
    physics.Step(universe, deltaTime);

    Console.ForegroundColor =
        ConsoleColor.Cyan;

    Console.WriteLine();
    Console.WriteLine(
        $"================ STEP {step} ================");

    Console.ResetColor();

    int index = 1;

    foreach (var body in universe.Bodies)
    {
        var speed =
            Math.Sqrt(
                body.Velocity.X * body.Velocity.X +
                body.Velocity.Y * body.Velocity.Y);

        if (speed > 100)
        {
            Console.ForegroundColor =
                ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor =
                ConsoleColor.Green;
        }

        Console.WriteLine(
            $"B{index} | " +
            $"Pos=({body.Position.X:F2}, {body.Position.Y:F2}) " +
            $"Vel=({body.Velocity.X:F2}, {body.Velocity.Y:F2}) " +
            $"Speed={speed:F2}");

        Console.ResetColor();

        index++;
    }

    Console.WriteLine();
}