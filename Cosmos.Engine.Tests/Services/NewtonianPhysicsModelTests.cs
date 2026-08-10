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
    public void Step_WhenBodiesAreVeryClose_ShouldProduceFiniteContinuousAcceleration()
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

        //
        /// Assert
        ////
        var firstAcceleration =
            capturingIntegrator
                .Accelerations[firstBody.Id];

        var secondAcceleration =
            capturingIntegrator
                .Accelerations[secondBody.Id];

        // Gravity must remain active during a close encounter.
        Assert.True(
            firstAcceleration.Magnitude() > 0);

        Assert.True(
            secondAcceleration.Magnitude() > 0);

        // Softening must prevent NaN and infinity from entering the state.
        Assert.True(
            double.IsFinite(firstAcceleration.X));

        Assert.True(
            double.IsFinite(secondAcceleration.X));

        // Equal source masses at opposite sides produce equal acceleration
        // magnitudes in opposite directions.
        Assert.Equal(
            firstAcceleration.Magnitude(),
            secondAcceleration.Magnitude(),
            precision: 6);

        Assert.True(firstAcceleration.X > 0);
        Assert.True(secondAcceleration.X < 0);
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
        ///
        const double distance = 0.02;
        const double softeningLength = 0.01;
        const double gravitationalConstant = 100;
        const double attractingMass = 1;

        var softenedDistanceSquared =
            distance * distance +
            softeningLength * softeningLength;

        var expectedAccelerationMagnitude =
            gravitationalConstant *
            attractingMass *
            distance /
            Math.Pow(
                softenedDistanceSquared,
                1.5);

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