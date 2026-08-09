using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;

namespace Cosmos.Engine.Tests.Services;

public sealed class NewtonianPhysicsModelTests
{
    [Fact]
    public void Step_WhenBodiesAreCloserThanMinimumDistance_ShouldIgnoreTheirGravity()
    {
        // Arrange
        // The first body is placed at the coordinate origin.
        var firstBody = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(1),
            name: "First Body",
            type: BodyType.Planet);

        // The second body is only 0.005 position units away.
        //
        // r  = 0.005
        // r² = 0.000025
        //
        // The current physics model ignores gravity whenever:
        //
        // r² < 0.0001
        //
        // Therefore, this pair falls inside the model's current
        // minimum-distance guard.
        var secondBody = new Body(
            position: new Vector3D(0.005, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(1),
            name: "Second Body",
            type: BodyType.Planet);

        var universe = new Universe();
        universe.AddBody(firstBody);
        universe.AddBody(secondBody);

        // This test double records the accelerations that the physics
        // model sends to the integrator. It does not move any bodies.
        //
        // Keeping the bodies stationary lets this test inspect only
        // the gravity calculation, without introducing integration
        // behavior into the result.
        var capturingIntegrator =
            new CapturingIntegrator();

        var physicsModel =
            new NewtonianPhysicsModel(
                capturingIntegrator);

        const double deltaTime = 1;

        // Act
        physicsModel.Step(
            universe,
            deltaTime);

        // Assert
        // Without the minimum-distance guard, the acceleration
        // magnitude produced by the second body would be:
        //
        // a = G × M / r²
        // a = 100 × 1 / 0.000025
        // a = 4,000,000 position-units / time-unit²
        //
        // The current guard skips this interaction completely,
        // so the integrator receives zero acceleration.
        Assert.Equal(
            0,
            capturingIntegrator
                .Accelerations[firstBody.Id]
                .Magnitude());

        Assert.Equal(
            0,
            capturingIntegrator
                .Accelerations[secondBody.Id]
                .Magnitude());
    }


    [Fact]
    public void Step_WhenBodiesAreOutsideMinimumDistance_ShouldProduceLargeAcceleration()
    {
        // Arrange
        // The bodies are separated by 0.02 position units.
        //
        // r  = 0.02
        // r² = 0.0004
        //
        // Since 0.0004 is greater than the current cutoff value
        // of 0.0001, gravity will be calculated normally.
        var firstBody = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(1),
            name: "First Body",
            type: BodyType.Planet);

        var secondBody = new Body(
            position: new Vector3D(0.02, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(1),
            name: "Second Body",
            type: BodyType.Planet);

        var universe = new Universe();
        universe.AddBody(firstBody);
        universe.AddBody(secondBody);

        var capturingIntegrator =
            new CapturingIntegrator();

        var physicsModel =
            new NewtonianPhysicsModel(
                capturingIntegrator);

        const double deltaTime = 1;

        // Act
        physicsModel.Step(
            universe,
            deltaTime);

        // Assert
        // Acceleration produced by either body:
        //
        // a = G × M / r²
        // a = 100 × 1 / 0.0004
        // a = 250,000 position-units / time-unit²
        //
        // This is already five times greater than the integrator's
        // current maximum stored speed of 50,000.
        const double expectedAccelerationMagnitude =
            250_000;

        Assert.Equal(
            expectedAccelerationMagnitude,
            capturingIntegrator
                .Accelerations[firstBody.Id]
                .Magnitude(),
            precision: 6);

        Assert.Equal(
            expectedAccelerationMagnitude,
            capturingIntegrator
                .Accelerations[secondBody.Id]
                .Magnitude(),
            precision: 6);
    }

    private sealed class CapturingIntegrator
        : IIntegrator
    {
        public Dictionary<Guid, Vector3D> Accelerations
        {
            get;
        } = [];

        public void Integrate(
            Body body,
            Vector3D acceleration,
            double deltaTime)
        {
            // Record the acceleration calculated for each body.
            //
            // We intentionally do not update velocity or position:
            // this test is about the output of NewtonianPhysicsModel,
            // not the numerical behavior of an integrator.
            Accelerations[body.Id] =
                acceleration;
        }
    }
}