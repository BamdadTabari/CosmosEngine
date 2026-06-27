using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Calculators;

public sealed class EscapeVelocityCalculator
{
    private const double G = 100;

    /// <summary>
    /// vescape​=math.sqrt(2GM / r)
    /// </summary>
    /// <param name="body"></param>
    /// <param name="centralBody"></param>
    /// <returns></returns>
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

        var distance =
            Math.Sqrt(
                dx * dx +
                dy * dy +
                dz * dz);

        return Math.Sqrt(
            (2 *
             G *
             centralBody.Mass.Value)
            / distance);
    }
}