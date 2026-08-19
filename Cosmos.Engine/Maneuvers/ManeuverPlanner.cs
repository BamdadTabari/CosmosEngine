using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Calculators;

namespace Cosmos.Engine.Maneuvers;

public sealed class ManeuverPlanner
{
    private readonly HohmannTransferCalculator
        _hohmannCalculator = new();

    public ManeuverPlan PlanTransfer(
        double currentOrbitRadius,
        double targetOrbitRadius,
        ReferenceContext referenceContext)
    {
        var transfer =
            _hohmannCalculator.Calculate(
                currentOrbitRadius,
                targetOrbitRadius);

        return new ManeuverPlan(
            transfer.DeltaV1,
            transfer.DeltaV2,
            transfer.TotalDeltaV,
            transfer.TransferTime,
            referenceContext);
    }
}