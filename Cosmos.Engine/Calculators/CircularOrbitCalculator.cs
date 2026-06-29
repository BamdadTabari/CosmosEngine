namespace Cosmos.Engine.Calculators;

public sealed class CircularOrbitCalculator
{
    public double CalculateVelocity(
        double mu,
        double radius)
    {
        return Math.Sqrt(
            mu / radius);
    }
}