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

    [Fact]
    public void Update_WhenPaused_ShouldNotAdvanceOrAccumulateTime()
    {
        // Arrange: create a stopped universe with a one-unit fixed step.
        var physicsModel =
            new CountingPhysicsModel();

        var runner =
            new SimulationRunner(
                physicsModel,
                new OrbitalTracker(),
                new ManeuverExecutionSystem(),
                fixedDeltaTime: 1)
            {
                IsPaused = true
            };

        var universe =
            new Universe();

        var burnState =
            new BurnExecutionState();

        // Act: real time passes while simulation time remains frozen.
        runner.Update(
            universe,
            realDeltaTime: 10,
            burnTarget: null,
            currentPlan: null,
            burnState);

        // Assert: neither physics nor the simulation clock may advance.
        Assert.Equal(0, physicsModel.StepCount);
        Assert.Equal(0, runner.SimulationTime);

        // Act: resume with less than one complete simulation step.
        runner.IsPaused = false;

        runner.Update(
            universe,
            realDeltaTime: 0.5,
            burnTarget: null,
            currentPlan: null,
            burnState);

        // Assert: paused time was discarded rather than accumulated.
        Assert.Equal(0, physicsModel.StepCount);
        Assert.Equal(0, runner.SimulationTime);
    }

    [Fact]
    public void Update_WhenTimeScaleIsApplied_ShouldAdvanceScaledSimulationTime()
    {
        // Arrange: run simulation time three times faster than real time.
        var physicsModel =
            new CountingPhysicsModel();

        var runner =
            new SimulationRunner(
                physicsModel,
                new OrbitalTracker(),
                new ManeuverExecutionSystem(),
                fixedDeltaTime: 1)
            {
                TimeScale = 3
            };

        var universe =
            new Universe();

        var burnState =
            new BurnExecutionState();

        // Act: two real seconds become six simulation seconds.
        runner.Update(
            universe,
            realDeltaTime: 2,
            burnTarget: null,
            currentPlan: null,
            burnState);

        // Assert: time scaling changes speed, not fixed-step size.
        Assert.Equal(6, physicsModel.StepCount);
        Assert.Equal(6, runner.SimulationTime);
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