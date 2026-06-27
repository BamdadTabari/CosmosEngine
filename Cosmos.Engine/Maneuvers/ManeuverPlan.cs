namespace Cosmos.Engine.Models;

public sealed record ManeuverPlan(
    double DeltaV1,
    double DeltaV2,
    double TotalDeltaV,
    double TransferTime);