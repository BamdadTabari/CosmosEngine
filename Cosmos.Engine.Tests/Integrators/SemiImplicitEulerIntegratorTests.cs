using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Integrators;

namespace Cosmos.Engine.Tests.Integrators;

public sealed class SemiImplicitEulerIntegratorTests
{
    [Fact]
    public void Integrate_WhenVelocityIsLarge_ShouldPreserveCalculatedVelocityAndPosition()
    {
        // Arrange
        // The body starts from rest and receives a constant acceleration
        // of 60,000 simulation length units per time unit².
        //
        // With a timestep of one simulation time unit:
        //
        // v_new = v_old + a × Δt
        // v_new = 0 + 60,000 × 1
        // v_new = 60,000
        //
        // Semi-implicit Euler then uses the new velocity:
        //
        // x_new = x_old + v_new × Δt
        // x_new = 0 + 60,000 × 1
        // x_new = 60,000
        var body = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(1),
            name: "Test Body",
            type: BodyType.Planet);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var acceleration =
            new Vector3D(60_000, 0, 0);

        const double deltaTime = 1;

        // Act
        integrator.Integrate(
            body,
            acceleration,
            deltaTime);

        // Assert
        // The Newtonian integrator must return the velocity produced by
        // its integration equation without applying an arbitrary limit.
        //
        // A very large result may indicate an unsuitable timestep or a
        // close gravitational encounter, but silently changing that result
        // would hide the numerical problem instead of diagnosing it.
        Assert.Equal(
            60_000,
            body.Velocity.Magnitude());

        // Semi-implicit Euler updates position using the newly calculated
        // velocity, so the position must change by 60,000 units as well.
        Assert.Equal(
            60_000,
            body.Position.Magnitude());

        // Body.Acceleration records the acceleration actually used in
        // this timestep, which keeps the body's final state internally
        // understandable for diagnostics and rendering.
        Assert.Equal(
            60_000,
            body.Acceleration.Magnitude());
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

    [Theory]
    [InlineData(0)]
    [InlineData(-0.001)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Integrate_WhenDeltaTimeIsInvalid_ShouldThrow(
    double invalidDeltaTime)
    {
        // Arrange
        // Semi-implicit Euler requires a finite and strictly positive
        // timestep. Zero, negative, NaN, or infinite time intervals
        // do not represent a supported simulation step.
        var body = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(1, 0, 0),
            mass: new Mass(1),
            name: "Test Body",
            type: BodyType.Planet);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var acceleration =
            new Vector3D(0, 0, 0);

        // Act
        Action integrate = () =>
            integrator.Integrate(
                body,
                acceleration,
                invalidDeltaTime);

        // Assert
        // Rejecting the value before calculation prevents an invalid
        // timestep from contaminating position and velocity.
        Assert.Throws<ArgumentOutOfRangeException>(
            integrate);
    }

    [Fact]
    public void Integrate_WhenCalculatedStateIsNotFinite_ShouldThrowWithoutChangingBody()
    {
        // Arrange
        // double.MaxValue is finite, but multiplying it by a timestep
        // greater than one overflows the representable range of double.
        //
        // The position calculation becomes:
        //
        // x_new = 0 + double.MaxValue × 2
        // x_new = PositiveInfinity
        //
        // The integrator must detect this invalid result before writing
        // any part of the calculated state back into the body.
        var body = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(
                double.MaxValue,
                0,
                0),
            mass: new Mass(1),
            name: "Test Body",
            type: BodyType.Planet);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var acceleration =
            new Vector3D(0, 0, 0);

        const double deltaTime = 2;

        // Act
        Action integrate = () =>
            integrator.Integrate(
                body,
                acceleration,
                deltaTime);

        // Assert
        Assert.Throws<ArithmeticException>(
            integrate);

        // State mutation must be atomic:
        // if either calculated vector is invalid, neither position nor
        // velocity should be partially committed to the body.
        Assert.Equal(
            0,
            body.Position.X);

        Assert.Equal(
            double.MaxValue,
            body.Velocity.X);
    }
}