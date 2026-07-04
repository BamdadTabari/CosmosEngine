using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Calculators;

public sealed class CircularOrbitCalculator
{
    private const double G = 100;

    public double Calculate(
        Body body,
        Body centralBody)
    {
        var radius =
            (body.Position - centralBody.Position)
            .Magnitude();

        return Calculate(
            centralBody.Mass.Value,
            radius);
    }

    public double Calculate(
        double centralMass,
        double radius)
    {
        var mu =
            G * centralMass;

        return Math.Sqrt(
            mu / radius);
    }
}