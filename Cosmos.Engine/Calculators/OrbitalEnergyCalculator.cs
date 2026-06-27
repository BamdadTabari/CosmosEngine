using Cosmos.Domain.Entities;
using Cosmos.Engine.Models;

namespace Cosmos.Engine.Calculators;

public sealed class OrbitalEnergyCalculator
{
    private const double G = 100;

    public EnergyStatistics Calculate(
        Body body,
        Body centralBody)
    {
        var speed =
            Math.Sqrt(
                body.Velocity.X * body.Velocity.X +
                body.Velocity.Y * body.Velocity.Y +
                body.Velocity.Z * body.Velocity.Z);

        var distance =
            Math.Sqrt(
                Math.Pow(
                    body.Position.X -
                    centralBody.Position.X, 2)

                +

                Math.Pow(
                    body.Position.Y -
                    centralBody.Position.Y, 2)

                +

                Math.Pow(
                    body.Position.Z -
                    centralBody.Position.Z, 2));

        var kinetic =
            0.5 *
            body.Mass.Value *
            speed *
            speed;

        var potential =
            -(
                G *
                centralBody.Mass.Value *
                body.Mass.Value)
            / distance;

        return new EnergyStatistics(
            kinetic,
            potential,
            kinetic + potential);
    }
}