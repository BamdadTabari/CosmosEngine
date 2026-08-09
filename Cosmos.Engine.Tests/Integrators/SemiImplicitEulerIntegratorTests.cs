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
    public void Integrate_WhenPositionExceedsMaximumDistance_ShouldClampBodyToBoundary()
    {
        // Arrange
        // The body starts close to the current artificial boundary
        // and moves far enough to cross it during this timestep.
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
        // Without the clamp, the new X position would be 5,010,000.
        // The current implementation projects the body back onto
        // the artificial spherical boundary at radius 5,000,000.
        Assert.Equal(
            5_000_000,
            body.Position.Magnitude());

        // The boundary changes position but does not alter velocity.
        // Therefore, this is not a physical collision response.
        Assert.Equal(
            20_000,
            body.Velocity.Magnitude());
    }
}