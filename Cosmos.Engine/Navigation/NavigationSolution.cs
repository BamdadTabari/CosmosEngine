using Cosmos.Domain.Navigation;
using Cosmos.Engine.Analysis;

namespace Cosmos.Engine.Navigation;

public sealed class NavigationSolution
{
    public OrbitAnalysis Orbit { get; }

    public INavigationTarget Target { get; }

    public double DistanceToTarget { get; }

    public double RelativeSpeed { get; }

    public NavigationSolution(
        OrbitAnalysis orbit,
        INavigationTarget target,
        double distanceToTarget,
        double relativeSpeed)
    {
        Orbit = orbit;
        Target = target;
        DistanceToTarget = distanceToTarget;
        RelativeSpeed = relativeSpeed;
    }
}