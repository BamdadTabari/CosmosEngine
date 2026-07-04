using Cosmos.Domain.Entities;
using Cosmos.Engine.Calculators;

namespace Cosmos.Engine.Analysis;

public sealed class OrbitAnalyzer
{
    private readonly OrbitalStatisticsCalculator _statisticsCalculator;
    private readonly OrbitalEnergyCalculator _energyCalculator;
    private readonly SphereOfInfluenceStatisticsCalculator _influenceCalculator;
    private readonly CircularOrbitCalculator _circularOrbitCalculator;
    private readonly EscapeVelocityCalculator _escapeVelocityCalculator;

    public OrbitAnalyzer(
        OrbitalStatisticsCalculator statisticsCalculator,
        OrbitalEnergyCalculator energyCalculator,
        SphereOfInfluenceStatisticsCalculator influenceCalculator,
        CircularOrbitCalculator circularOrbitCalculator,
        EscapeVelocityCalculator escapeVelocityCalculator)
    {
        _statisticsCalculator = statisticsCalculator;
        _energyCalculator = energyCalculator;
        _influenceCalculator = influenceCalculator;
        _circularOrbitCalculator = circularOrbitCalculator;
        _escapeVelocityCalculator = escapeVelocityCalculator;
    }

    public OrbitAnalysis Analyze(
        Universe universe,
        Spacecraft spacecraft)
    {
        Body primaryBody =
            universe.Bodies.First();

        var statistics =
            _statisticsCalculator.Calculate(spacecraft);

        var energy =
            _energyCalculator.Calculate(
                spacecraft,
                primaryBody);

        var influence =
            _influenceCalculator.Calculate(
                spacecraft,
                primaryBody);

        var dx =
            spacecraft.Position.X -
            primaryBody.Position.X;

        var dy =
            spacecraft.Position.Y -
            primaryBody.Position.Y;

        var dz =
            spacecraft.Position.Z -
            primaryBody.Position.Z;

        var radius =
            Math.Sqrt(
                dx * dx +
                dy * dy +
                dz * dz);

        var mu =
            100 *
            primaryBody.Mass.Value;

        double circularVelocity =
    _circularOrbitCalculator.Calculate(
        spacecraft,
        primaryBody);

        double escapeVelocity =
            _escapeVelocityCalculator.Calculate(
                spacecraft,
                primaryBody);

        return new OrbitAnalysis(
            statistics,
            energy,
            influence,
            circularVelocity,
            escapeVelocity);
    }
}