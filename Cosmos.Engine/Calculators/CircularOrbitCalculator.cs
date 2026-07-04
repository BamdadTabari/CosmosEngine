using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Calculators;

public sealed class CircularOrbitCalculator
{
    private const double G = 100;

    public double Calculate(
        Body body,
        Body centralBody)
    {
        var dx =
            body.Position.X -
            centralBody.Position.X;

        var dy =
            body.Position.Y -
            centralBody.Position.Y;

        var dz =
            body.Position.Z -
            centralBody.Position.Z;

        var radius =
            Math.Sqrt(
                dx * dx +
                dy * dy +
                dz * dz);

        var mu =
            G *
            centralBody.Mass.Value;

        return Math.Sqrt(
            mu /
            radius);
    }
}