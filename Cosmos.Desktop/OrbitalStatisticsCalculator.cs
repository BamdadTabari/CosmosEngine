using Cosmos.Domain.Entities;

namespace Cosmos.Desktop;

public sealed class OrbitalStatisticsCalculator
{
    public OrbitalStatistics Calculate(
        Body body)
    {
        var distance =
            Math.Sqrt(
                body.Position.X * body.Position.X +
                body.Position.Y * body.Position.Y +
                body.Position.Z * body.Position.Z);

        var speed =
            Math.Sqrt(
                body.Velocity.X * body.Velocity.X +
                body.Velocity.Y * body.Velocity.Y +
                body.Velocity.Z * body.Velocity.Z);

        var acceleration =
            Math.Sqrt(
                body.Acceleration.X * body.Acceleration.X +
                body.Acceleration.Y * body.Acceleration.Y +
                body.Acceleration.Z * body.Acceleration.Z);

        return new OrbitalStatistics(
            distance,
            speed,
            acceleration);
    }
}