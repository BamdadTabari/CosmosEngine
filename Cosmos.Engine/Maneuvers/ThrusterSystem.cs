using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Maneuvers;

public sealed class ThrusterSystem
{
    public void ProgradeBurn(
        Body body,
        double deltaV)
    {
        var direction =
            body.Velocity.Normalize();

        body.SetVelocity(
            body.Velocity +
            direction * deltaV);
    }

    public void RetrogradeBurn(
        Body body,
        double deltaV)
    {
        var direction =
            body.Velocity.Normalize();

        body.SetVelocity(
            body.Velocity -
            direction * deltaV);
    }
}