namespace Cosmos.Domain.OrbitalMechanics;

public sealed class Orbit
{
    public double Apoapsis { get; }

    public double Periapsis { get; }

    public double SemiMajorAxis { get; }

    public double Eccentricity { get; }

    public double Period { get; }

    public Orbit(
        double apoapsis,
        double periapsis,
        double semiMajorAxis,
        double eccentricity,
        double period)
    {
        Apoapsis = apoapsis;
        Periapsis = periapsis;
        SemiMajorAxis = semiMajorAxis;
        Eccentricity = eccentricity;
        Period = period;
    }
}