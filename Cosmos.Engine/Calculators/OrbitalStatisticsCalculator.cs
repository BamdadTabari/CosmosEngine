using Cosmos.Domain.Entities;
using Cosmos.Engine.Models;
using Cosmos.Engine.Tracking;

namespace Cosmos.Engine.Calculators;

public sealed class OrbitalStatisticsCalculator
{
    private readonly OrbitalTracker _tracker;

    public OrbitalStatisticsCalculator(
        OrbitalTracker tracker)
    {
        _tracker = tracker;
    }

    public OrbitalStatistics Calculate(
        Body body)
    {
        // Distance from simulation center (Sun)
        // r = √(x² + y² + z²)
        var distance =
            Math.Sqrt(
                body.Position.X * body.Position.X +
                body.Position.Y * body.Position.Y +
                body.Position.Z * body.Position.Z);

        // Orbital speed magnitude
        // v = √(vx² + vy² + vz²)
        var speed =
            Math.Sqrt(
                body.Velocity.X * body.Velocity.X +
                body.Velocity.Y * body.Velocity.Y +
                body.Velocity.Z * body.Velocity.Z);

        // Current acceleration magnitude
        // a = √(ax² + ay² + az²)
        var acceleration =
            Math.Sqrt(
                body.Acceleration.X * body.Acceleration.X +
                body.Acceleration.Y * body.Acceleration.Y +
                body.Acceleration.Z * body.Acceleration.Z);

        var periapsis =
            _tracker.GetPeriapsis(body);

        var apoapsis =
            _tracker.GetApoapsis(body);

        double eccentricity = 0;

        if (apoapsis > 0)
        {
            eccentricity =
                (apoapsis - periapsis) /
                (apoapsis + periapsis);
        }

        return new OrbitalStatistics(
            distance,
            speed,
            acceleration,
            eccentricity,
            periapsis,
            apoapsis);
    }
}