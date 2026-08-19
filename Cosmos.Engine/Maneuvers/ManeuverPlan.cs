using Cosmos.Domain.ValueObjects;

namespace Cosmos.Engine.Maneuvers;

public sealed record ManeuverPlan(
    double DeltaV1,
    double DeltaV2,
    double TotalDeltaV,
    double TransferTime,
    ReferenceContext ReferenceContext);