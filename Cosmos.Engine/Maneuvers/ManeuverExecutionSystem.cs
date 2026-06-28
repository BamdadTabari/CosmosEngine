using Cosmos.Domain.Entities;
using Cosmos.Engine.Maneuvers;

public sealed class ManeuverExecutionSystem
{
    public void Update(
        Body body,
        ManeuverPlan? plan,
        double dt,
        int burnStep,
        double burnTimer)
    {
        if (plan is null)
            return;

        // 🔥 Burn1
        if (burnStep == 0)
        {
            ApplyBurn(body, plan.DeltaV1);
            burnStep = 1;
        }

        // ⏳ wait
        else if (burnStep == 1)
        {
            burnTimer += dt;

            if (burnTimer > plan.TransferTime * 0.5)
            {
                burnStep = 2;
            }
        }

        // 🔥 Burn2
        else if (burnStep == 2)
        {
            ApplyBurn(body, plan.DeltaV2);
            burnStep = 3;
        }
    }

    private void ApplyBurn(Body body, double dv)
    {
        var pos = body.Position;
        var vel = body.Velocity;

        // 1. direction from center (Sun assumed at 0,0,0)
        var radial = pos.Normalize();

        // 2. tangent direction (rotate 90 degrees in orbit plane)
        var tangent = new Cosmos.Domain.Structs.Vector3D(
            -radial.Y,
             radial.X,
             0).Normalize();

        // 3. apply burn along tangent
        body.SetVelocity(
            vel + tangent * dv);
    }
}