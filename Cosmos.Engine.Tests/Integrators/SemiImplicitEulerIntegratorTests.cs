using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Integrators;

namespace Cosmos.Engine.Tests.Integrators;

public sealed class SemiImplicitEulerIntegratorTests
{
    [Fact]
    public void Integrate_WhenCalculatedVelocityExceedsMaximumSpeed_ShouldCalculatePositionBeforeVelocityClamp()
    {
        // Arrange
        // With zero acceleration and a one-second timestep,
        // the calculated velocity remains 60,000 units per second.
        // This exceeds the integrator's current maximum speed of 50,000.
        var body = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(60_000, 0, 0),
            mass: new Mass(1),
            name: "Test Body",
            type: BodyType.Planet);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var acceleration =
            new Vector3D(0, 0, 0);

        const double deltaTime = 1;

        // Act
        integrator.Integrate(
            body,
            acceleration,
            deltaTime);

        // Assert
        // The current implementation calculates position using the
        // unclamped velocity, so the body moves 60,000 units.
        Assert.Equal(
            60_000,
            body.Position.Magnitude());

        // Velocity is clamped only after position has been calculated.
        Assert.Equal(
            50_000,
            body.Velocity.Magnitude());
    }

    [Fact]
    public void Integrate_WhenBodyMovesFarFromOrigin_ShouldNotAlterCalculatedPosition()
    {
        // Arrange
        // The body starts at 4,990,000 simulation length units
        // from the coordinate origin.
        //
        // With a velocity of 20,000 length units per time unit
        // and a timestep of one time unit, its expected position is:
        //
        // x_new = x_old + v_new × Δt
        // x_new = 4,990,000 + 20,000 × 1
        // x_new = 5,010,000
        var body = new Body(
            position: new Vector3D(4_990_000, 0, 0),
            velocity: new Vector3D(20_000, 0, 0),
            mass: new Mass(1),
            name: "Test Body",
            type: BodyType.Planet);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var acceleration =
            new Vector3D(0, 0, 0);

        const double deltaTime = 1;

        // Act
        integrator.Integrate(
            body,
            acceleration,
            deltaTime);

        // Assert
        // Newtonian space has no artificial boundary in the current model.
        // The integrator must preserve its calculated position instead of
        // projecting the body back toward the coordinate origin.
        Assert.Equal(
            5_010_000,
            body.Position.Magnitude());

        // No acceleration was applied, so velocity remains unchanged.
        Assert.Equal(
            20_000,
            body.Velocity.Magnitude());
    }
}