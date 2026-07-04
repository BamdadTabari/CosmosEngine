using Cosmos.Engine.Models;

namespace Cosmos.Engine.Analysis;

public sealed class OrbitAnalysis
{
    public OrbitalStatistics Statistics { get; }

    public EnergyStatistics Energy { get; }

    public OrbitalInfluenceStatistics Influence { get; }

    public double CircularOrbitVelocity { get; }

    public double EscapeVelocity { get; }

    public OrbitAnalysis(
        OrbitalStatistics statistics,
        EnergyStatistics energy,
        OrbitalInfluenceStatistics influence,
        double circularOrbitVelocity,
        double escapeVelocity)
    {
        Statistics = statistics;
        Energy = energy;
        Influence = influence;
        CircularOrbitVelocity = circularOrbitVelocity;
        EscapeVelocity = escapeVelocity;
    }
}