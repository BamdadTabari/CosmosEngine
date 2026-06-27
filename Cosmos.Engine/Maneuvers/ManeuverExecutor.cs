using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;

namespace Cosmos.Engine.Maneuvers;

public sealed class ManeuverExecutor
{
    public void ExecuteFirstBurn(
        Body body,
        ManeuverPlan plan)
    {
        var velocity =
            body.Velocity;

        var speed =
            velocity.Magnitude();

        if (speed <= 0)
            return;

        var direction =
            velocity.Normalize();

        var newVelocity =
            velocity +
            direction * plan.DeltaV1;

        body.SetVelocity(
            newVelocity);
    }
}