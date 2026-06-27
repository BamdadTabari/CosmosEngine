using Cosmos.Domain.Entities;
using Cosmos.Engine.Models;

namespace Cosmos.Engine.Calculators;

public sealed class SphereOfInfluenceStatisticsCalculator
{
    private readonly SphereOfInfluenceCalculator
        _calculator = new();

    public OrbitalInfluenceStatistics Calculate(
        Body target,
        Body parent)
    {
        var soi =
            _calculator.Calculate(
                parent,
                target);

        var distance =
            target.Position.DistanceTo(
                parent.Position);

        return new OrbitalInfluenceStatistics(
            parent.Name,
            soi,
            distance <= soi);
    }
}