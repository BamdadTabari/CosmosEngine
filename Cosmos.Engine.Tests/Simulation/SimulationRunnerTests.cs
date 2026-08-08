using Cosmos.Domain.Entities;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Integrators;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Models;
using Cosmos.Engine.Services;
using Cosmos.Engine.Simulation;
using Cosmos.Engine.Tracking;

namespace Cosmos.Engine.Tests.Simulation;

public sealed class SimulationRunnerTests
{
    [Fact]
    public void Constructor_WhenFixedDeltaTimeIsZero_ShouldThrow()
    {
        // Arrange: build the smallest real simulation pipeline.
        var integrator =
            new SemiImplicitEulerIntegrator();

        var physicsModel =
            new NewtonianPhysicsModel(integrator);

        var orbitalTracker =
            new OrbitalTracker();

        var maneuverExecutionSystem =
            new ManeuverExecutionSystem();

        // Act: a universe cannot advance with a zero-sized moment.
        Action createRunner = () =>
            new SimulationRunner(
                physicsModel,
                orbitalTracker,
                maneuverExecutionSystem,
                fixedDeltaTime: 0);

        // Assert: reject the invalid clock before simulation begins.
        Assert.Throws<ArgumentOutOfRangeException>(
            createRunner);
    }

    [Fact]
    public void Update_WhenTimeContainsRemainder_ShouldPreserveItForNextUpdate()
    {
        // Arrange: isolate the simulation clock from real physics.
        var physicsModel =
            new CountingPhysicsModel();

        var runner =
            new SimulationRunner(
                physicsModel,
                new OrbitalTracker(),
                new ManeuverExecutionSystem(),
                fixedDeltaTime: 1);

        var universe =
            new Universe();

        var burnState =
            new BurnExecutionState();

        // Act: three complete steps pass; half a step remains accumulated.
        runner.Update(
            universe,
            realDeltaTime: 3.5,
            burnTarget: null,
            currentPlan: null,
            burnState);

        // Assert: only complete fixed steps advance the universe.
        Assert.Equal(3, physicsModel.StepCount);
        Assert.Equal(3, runner.SimulationTime);

        // Act: the next half-step completes the stored remainder.
        runner.Update(
            universe,
            realDeltaTime: 0.5,
            burnTarget: null,
            currentPlan: null,
            burnState);

        // Assert: no fragment of elapsed time was lost.
        Assert.Equal(4, physicsModel.StepCount);
        Assert.Equal(4, runner.SimulationTime);
    }

    private sealed class CountingPhysicsModel
        : IPhysicsModel
    {
        public int StepCount { get; private set; }

        public void Step(
            Universe universe,
            double deltaTime)
        {
            StepCount++;
        }
    }
}