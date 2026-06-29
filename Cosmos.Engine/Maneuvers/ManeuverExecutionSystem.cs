using Cosmos.Domain.Entities;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Models;

namespace Cosmos.Engine.Maneuvers;

public sealed class ManeuverExecutionSystem
{
    public void Update(
    Body body,
    ManeuverPlan? plan,
    double dt,
    BurnExecutionState burnState)
    {
        if (plan is null)
            return;

        // 🔥 Burn1
        if (burnState.BurnStep == 0)
        {
            ApplyBurn(body, plan.DeltaV1);
            burnState.BurnStep = 1;
        }

        // ⏳ wait
        else if (burnState.BurnStep == 1)
        {
            burnState.BurnTimer += dt;

            if (burnState.BurnTimer > plan.TransferTime * 0.5)
            {
                burnState.BurnStep = 2;
            }
        }

        // 🔥 Burn2
        else if (burnState.BurnStep == 2)
        {
            ApplyBurn(body, plan.DeltaV2);
            burnState.BurnStep = 3;
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