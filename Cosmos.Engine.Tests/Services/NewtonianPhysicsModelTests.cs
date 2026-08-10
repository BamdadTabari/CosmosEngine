using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Integrators;
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


    [Fact]
    public void Step_AtEarthScale_ShouldRemainCloseToNewtonianGravity()
    {
        // Arrange
        // These values match the normalized scale currently used by
        // solar-system.json:
        //
        // Sun mass      = 100,000
        // Earth distance = 150
        // G              = 100
        //
        // The softening length is only 0.01, which is extremely small
        // compared with the Earth-Sun distance. Therefore, softened
        // gravity should remain almost identical to Newtonian gravity.
        var sun = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(100_000),
            name: "Sun",
            type: BodyType.Star);

        var earth = new Body(
            position: new Vector3D(150, 0, 0),
            velocity: new Vector3D(0, 260, 0),
            mass: new Mass(10),
            name: "Earth",
            type: BodyType.Planet);

        var universe = new Universe();
        universe.AddBody(sun);
        universe.AddBody(earth);

        var capturingIntegrator =
            new CapturingIntegrator();

        var physicsModel =
            new NewtonianPhysicsModel(
                capturingIntegrator);

        const double deltaTime = 0.001;

        // Expected acceleration from ordinary Newtonian gravity:
        //
        // a = G × M / r²
        // a = 100 × 100,000 / 150²
        // a ≈ 444.444444
        const double gravitationalConstant = 100;
        const double sunMass = 100_000;
        const double distance = 150;

        var expectedNewtonianAcceleration =
            gravitationalConstant *
            sunMass /
            (distance * distance);

        // Act
        physicsModel.Step(
            universe,
            deltaTime);

        var actualSoftenedAcceleration =
            capturingIntegrator
                .Accelerations[earth.Id]
                .Magnitude();

        var relativeDifference =
            Math.Abs(
                actualSoftenedAcceleration -
                expectedNewtonianAcceleration) /
            expectedNewtonianAcceleration;

        // Assert
        // Softening must not materially alter gravity at ordinary orbital
        // distances. A relative difference below 1e-8 means the change is
        // less than one part in one hundred million.
        Assert.True(
            relativeDifference < 1e-8,
            $"Relative difference was {relativeDifference:E6}.");

        // Earth is positioned on the positive X axis, so gravitational
        // acceleration must point back toward the Sun on negative X.
        Assert.True(
            capturingIntegrator
                .Accelerations[earth.Id]
                .X < 0);
    }

    [Fact]
    public void Step_ForEarthLikeInitialConditions_ShouldKeepOrbitBounded()
    {
        // Arrange
        // This is an integration-style scientific test:
        // NewtonianPhysicsModel calculates acceleration and
        // SemiImplicitEulerIntegrator advances the bodies.
        //
        // Unlike tests using CapturingIntegrator, this test verifies
        // their combined behavior over many consecutive timesteps.
        var sun = new Body(
            position: new Vector3D(0, 0, 0),
            velocity: new Vector3D(0, 0, 0),
            mass: new Mass(100_000),
            name: "Sun",
            type: BodyType.Star);

        var earth = new Body(
            position: new Vector3D(150, 0, 0),
            velocity: new Vector3D(0, 260, 0),
            mass: new Mass(10),
            name: "Earth",
            type: BodyType.Planet);

        var universe = new Universe();
        universe.AddBody(sun);
        universe.AddBody(earth);

        var integrator =
            new SemiImplicitEulerIntegrator();

        var physicsModel =
            new NewtonianPhysicsModel(
                integrator);

        // The production simulation uses the same fixed timestep.
        //
        // A small fixed timestep reduces discretization error and makes
        // repeated runs deterministic.
        const double deltaTime = 0.001;

        // Ten simulation time units cover multiple Earth-like orbits.
        //
        // The approximate orbital period is:
        //
        // T ≈ 2πr / v
        // T ≈ 2π × 150 / 260
        // T ≈ 3.62 simulation time units
        //
        // Therefore, 10,000 steps cover roughly 2.7 orbits.
        const int stepCount = 10_000;

        var minimumDistance =
            double.PositiveInfinity;

        var maximumDistance =
            double.NegativeInfinity;

        // Act
        for (var step = 0;
             step < stepCount;
             step++)
        {
            physicsModel.Step(
                universe,
                deltaTime);

            // Measure relative separation rather than Earth's distance
            // from the coordinate origin, because the Sun is also allowed
            // to respond to Earth's gravity and move slightly.
            var separation =
                (earth.Position - sun.Position)
                .Magnitude();

            minimumDistance =
                Math.Min(
                    minimumDistance,
                    separation);

            maximumDistance =
                Math.Max(
                    maximumDistance,
                    separation);
        }

        // Assert
        // The current initial speed is slightly greater than the exact
        // circular-orbit speed, so a small radial variation is expected.
        //
        // These bounds intentionally verify qualitative orbital stability
        // rather than demanding an unrealistically perfect circle.
        Assert.True(
            minimumDistance > 145,
            $"Minimum separation was {minimumDistance:F6}.");

        Assert.True(
            maximumDistance < 160,
            $"Maximum separation was {maximumDistance:F6}.");

        // Explicit finite checks make numerical explosions easier to
        // diagnose than a later failure in rendering or tracking.
        Assert.True(
            double.IsFinite(minimumDistance));

        Assert.True(
            double.IsFinite(maximumDistance));
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