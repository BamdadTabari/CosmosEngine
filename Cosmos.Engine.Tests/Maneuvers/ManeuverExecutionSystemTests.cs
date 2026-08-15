using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Models;

namespace Cosmos.Engine.Tests.Maneuvers;

public sealed class ManeuverExecutionSystemTests
{
    [Fact]
    public void Update_WhenFirstBurnExecutes_ShouldChangeOnlyProvidedBodyVelocity()
    {
        // Arrange
        // Explorer-1 is the maneuvering body.
        //
        // It starts on the positive X axis, so in the current simplified
        // XY-plane model the prograde tangent points along positive Y.
        var spacecraft = new Body(
            position: new Vector3D(100, 0, 0),
            velocity: new Vector3D(0, 10, 0),
            mass: new Mass(1),
            name: "Explorer-1",
            type: BodyType.Spacecraft);

        // This body represents another object in the simulation.
        //
        // A maneuver intended for Explorer-1 must not modify it.
        var unrelatedBody = new Body(
            position: new Vector3D(150, 0, 0),
            velocity: new Vector3D(0, 20, 0),
            mass: new Mass(10),
            name: "Earth",
            type: BodyType.Planet);

        var unrelatedInitialVelocity =
            unrelatedBody.Velocity;

        var plan = new ManeuverPlan(
            DeltaV1: 5,
            DeltaV2: 2,
            TotalDeltaV: 7,
            TransferTime: 10);

        var burnState =
            new BurnExecutionState();

        var system =
            new ManeuverExecutionSystem();

        // Act
        // Execute the first impulsive burn specifically on Explorer-1.
        system.Update(
            spacecraft,
            plan,
            dt: 0.001,
            burnState);

        // Assert
        // Initial spacecraft velocity:
        //
        //     v = (0, 10, 0)
        //
        // The first prograde burn adds:
        //
        //     Δv = (0, 5, 0)
        //
        // Therefore:
        //
        //     v_new = (0, 15, 0)
        Assert.Equal(
            new Vector3D(0, 15, 0),
            spacecraft.Velocity);

        // The unrelated body must remain completely unaffected.
        Assert.Equal(
            unrelatedInitialVelocity,
            unrelatedBody.Velocity);

        // Burn #1 has completed, so execution advances to the waiting stage.
        Assert.Equal(
            1,
            burnState.BurnStep);
    }
}