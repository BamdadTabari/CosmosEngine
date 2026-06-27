using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Maneuvers;

public sealed class ManeuverPlanner
{
    public ManeuverPlan PlanCircularization(
        Body body)
    {
        var speed =
            Math.Sqrt(
                body.Velocity.X * body.Velocity.X +
                body.Velocity.Y * body.Velocity.Y +
                body.Velocity.Z * body.Velocity.Z);

        return new ManeuverPlan(
            speed * 0.05,
            "Circularization Burn");
    }
}