using Cosmos.Engine.Maneuvers;

namespace Cosmos.Engine.Calculators;

/// <summary>
/// Most fuel-efficient transfer between two circular orbits.
///
/// Burn #1:
/// Enter transfer ellipse.
///
/// Burn #2:
/// Circularize at destination orbit.
///
/// Common use cases:
/// - Orbit raising
/// - Orbit lowering
/// - Interplanetary mission planning
/// </summary>
public sealed class HohmannTransferCalculator
{
    private const double Mu = 10_000_000;

    public HohmannTransfer Calculate(
        double startOrbitRadius,
        double targetOrbitRadius)
    {
        var transferSemiMajorAxis =
            (startOrbitRadius + targetOrbitRadius) / 2.0;

        var circularVelocity1 =
            Math.Sqrt(
                Mu / startOrbitRadius);

        var transferVelocity1 =
            Math.Sqrt(
                Mu *
                (
                    2.0 / startOrbitRadius -
                    1.0 / transferSemiMajorAxis
                ));

        var deltaV1 =
            Math.Abs(
                transferVelocity1 -
                circularVelocity1);

        var circularVelocity2 =
            Math.Sqrt(
                Mu / targetOrbitRadius);

        var transferVelocity2 =
            Math.Sqrt(
                Mu *
                (
                    2.0 / targetOrbitRadius -
                    1.0 / transferSemiMajorAxis
                ));

        var deltaV2 =
            Math.Abs(
                circularVelocity2 -
                transferVelocity2);

        var transferTime =
            Math.PI *
            Math.Sqrt(
                Math.Pow(
                    transferSemiMajorAxis,
                    3) / Mu);

        return new HohmannTransfer(
            deltaV1,
            deltaV2,
            deltaV1 + deltaV2,
            transferTime);
    }
}