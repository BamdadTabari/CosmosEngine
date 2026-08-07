using Cosmos.Engine.Integrators;
using Cosmos.Engine.Maneuvers;
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
}