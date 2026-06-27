namespace Cosmos.Engine.Maneuvers;

public sealed record HohmannTransfer(
    double DeltaV1,
    double DeltaV2,
    double TotalDeltaV,
    double TransferTime);