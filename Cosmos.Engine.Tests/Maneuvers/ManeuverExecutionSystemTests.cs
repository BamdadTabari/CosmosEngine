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

        var centralBody = new Body(
        position: new Vector3D(0, 0, 0),
        velocity: new Vector3D(0, 0, 0),
        mass: new Mass(100_000),
        name: "Sun",
        type: BodyType.Star);

        var referenceContext =
            new ReferenceContext(
                ReferenceFrame.BodyCentered,
                centralBody);

        var plan = new ManeuverPlan(
            DeltaV1: 5,
            DeltaV2: 2,
            TotalDeltaV: 7,
            TransferTime: 10,
            ReferenceContext: referenceContext);

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

    [Fact]
    public void Update_WhenCentralBodyIsOffsetFromOrigin_ShouldUseRelativeOrbitalDirection()
    {
        // Arrange
        // The Sun is deliberately NOT at the global origin.
        //
        // This prevents the test from accidentally passing because
        // "central body" and "coordinate origin" happen to coincide.
        var sun = new Body(
            position: new Vector3D(100, 100, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(100_000),
            name: "Sun",
            type: BodyType.Star);

        // Relative spacecraft position:
        //
        //     (100, 200, 0)
        //   - (100, 100, 0)
        //   ----------------
        //     (0, 100, 0)
        //
        // Therefore the normalized radial direction is:
        //
        //     r̂ = (0, 1, 0)
        //
        // Rotating that +90° in the current XY-plane model gives:
        //
        //     t̂ = (-1, 0, 0)
        var spacecraft = new Body(
            position: new Vector3D(100, 200, 0),
            velocity: new Vector3D(-10, 0, 0),
            mass: new Mass(1),
            name: "Explorer-1",
            type: BodyType.Spacecraft);

        var referenceContext =
            new ReferenceContext(
                ReferenceFrame.BodyCentered,
                sun);

        var plan = new ManeuverPlan(
            DeltaV1: 5,
            DeltaV2: 2,
            TotalDeltaV: 7,
            TransferTime: 10,
            ReferenceContext: referenceContext);

        var burnState =
            new BurnExecutionState();

        var system =
            new ManeuverExecutionSystem();

        // Act
        system.Update(
            spacecraft,
            plan,
            dt: 0.001,
            burnState);

        // Assert
        //
        // Initial velocity:
        //
        //     (-10, 0, 0)
        //
        // Burn:
        //
        //     5 × (-1, 0, 0)
        //     = (-5, 0, 0)
        //
        // Final:
        //
        //     (-15, 0, 0)
        Assert.Equal(
            new Vector3D(-15, 0, 0),
            spacecraft.Velocity);

        Assert.Equal(
            1,
            burnState.BurnStep);
    }

    [Fact]
    public void Update_WhenCentralBodyIsMoving_ShouldUseRelativeVelocityForProgradeDirection()
    {
        // Arrange
        // The Sun itself is moving through the global coordinate frame.
        var sun = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(100, 50, 0),
            mass: new Mass(100_000),
            name: "Sun",
            type: BodyType.Star);

        // Global spacecraft velocity:
        //
        //     v_ship = (100, 40, 0)
        //
        // Sun velocity:
        //
        //     v_sun = (100, 50, 0)
        //
        // Therefore the orbital velocity relative to the Sun is:
        //
        //     v_rel = v_ship - v_sun
        //           = (0, -10, 0)
        //
        // So the spacecraft is moving in the negative Y direction
        // relative to the Sun.
        var spacecraft = new Body(
            position: new Vector3D(100, 0, 0),
            velocity: new Vector3D(100, 40, 0),
            mass: new Mass(1),
            name: "Explorer-1",
            type: BodyType.Spacecraft);

        var referenceContext =
            new ReferenceContext(
                ReferenceFrame.BodyCentered,
                sun);

        var plan = new ManeuverPlan(
            DeltaV1: 5,
            DeltaV2: 2,
            TotalDeltaV: 7,
            TransferTime: 10,
            ReferenceContext: referenceContext);

        var burnState =
            new BurnExecutionState();

        var system =
            new ManeuverExecutionSystem();

        // Act
        system.Update(
            spacecraft,
            plan,
            dt: 0.001,
            burnState);

        // Assert
        //
        // Relative velocity before burn:
        //
        //     (0, -10, 0)
        //
        // Prograde Δv:
        //
        //     (0, -5, 0)
        //
        // Relative velocity after burn:
        //
        //     (0, -15, 0)
        //
        // Therefore global spacecraft velocity becomes:
        //
        //     Sun velocity + relative velocity
        //     (100, 50, 0) + (0, -15, 0)
        //     = (100, 35, 0)
        Assert.Equal(
            new Vector3D(100, 35, 0),
            spacecraft.Velocity);

        Assert.Equal(
            1,
            burnState.BurnStep);
    }
}