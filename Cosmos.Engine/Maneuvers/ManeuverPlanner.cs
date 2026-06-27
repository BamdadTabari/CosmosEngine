using Cosmos.Engine.Calculators;
using Cosmos.Engine.Models;

namespace Cosmos.Engine.Maneuvers;

public sealed class ManeuverPlanner
{

    private readonly HohmannTransferCalculator
    _hohmannCalculator = new();

    public ManeuverPlan PlanTransfer(
    double currentOrbitRadius,
    double targetOrbitRadius)
    {
        var transfer =
            _hohmannCalculator.Calculate(
                currentOrbitRadius,
                targetOrbitRadius);

        return new ManeuverPlan(
            transfer.DeltaV1,
            transfer.DeltaV2,
            transfer.TotalDeltaV,
            transfer.TransferTime);
    }
}