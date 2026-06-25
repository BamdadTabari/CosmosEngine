using Cosmos.Domain.Entities;

namespace Cosmos.Desktop;

public sealed class OrbitalStatisticsCalculator
{
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

        // -------------------------------------------------
        // Kepler First Law
        //
        // Planets move in elliptical orbits.
        //
        // Eccentricity tells us how stretched the ellipse is:
        //
        // e = 0      -> perfect circle
        // 0 < e < 1  -> ellipse
        // e = 1      -> parabola (escape)
        // e > 1      -> hyperbola (body escapes forever)
        //
        // For now we assume:
        // - Sun at origin
        // - G = 100
        // - Sun mass = 100000
        // -------------------------------------------------

        const double G = 100;
        const double SunMass = 100000;

        var mu =
            G * SunMass;

        // Specific orbital energy:
        //
        // ε = v²/2 - μ/r
        //
        // total energy per unit mass
        var specificEnergy =
            (speed * speed / 2.0) -
            (mu / distance);

        // Angular momentum magnitude:
        //
        // h = |r × v|
        //
        // determines how much orbital rotation exists
        var hx =
            body.Position.Y * body.Velocity.Z -
            body.Position.Z * body.Velocity.Y;

        var hy =
            body.Position.Z * body.Velocity.X -
            body.Position.X * body.Velocity.Z;

        var hz =
            body.Position.X * body.Velocity.Y -
            body.Position.Y * body.Velocity.X;

        var angularMomentum =
            Math.Sqrt(
                hx * hx +
                hy * hy +
                hz * hz);

        // Orbital eccentricity:
        //
        // e = sqrt(
        //      1 +
        //      (2 * ε * h²) / μ²
        // )
        //
        var eccentricity =
            Math.Sqrt(
                Math.Max(
                    0,
                    1 +
                    (
                        2 *
                        specificEnergy *
                        angularMomentum *
                        angularMomentum
                    ) /
                    (
                        mu * mu
                    )));

        return new OrbitalStatistics(
            distance,
            speed,
            acceleration,
            eccentricity);
    }
}