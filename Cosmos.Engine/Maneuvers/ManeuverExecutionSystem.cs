using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
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
        {
            return;
        }

        // The current Hohmann maneuver requires a body-centered
        // reference frame with an explicit physical origin.
        if (plan.ReferenceContext.Frame !=
                ReferenceFrame.BodyCentered ||
            plan.ReferenceContext.Origin is null)
        {
            return;
        }

        var centralBody =
            plan.ReferenceContext.Origin;

        // 🔥 Burn 1
        if (burnState.BurnStep == 0)
        {
            ApplyBurn(
                body,
                centralBody,
                plan.DeltaV1);

            burnState.BurnStep = 1;
        }

        // ⏳ Coast through the transfer orbit.
        else if (burnState.BurnStep == 1)
        {
            burnState.BurnTimer += dt;

            if (burnState.BurnTimer >
                plan.TransferTime * 0.5)
            {
                burnState.BurnStep = 2;
            }
        }

        // 🔥 Burn 2
        else if (burnState.BurnStep == 2)
        {
            ApplyBurn(
                body,
                centralBody,
                plan.DeltaV2);

            burnState.BurnStep = 3;
        }
    }

    private static void ApplyBurn(
      Body body,
      Body centralBody,
      double deltaV)
    {
        var velocity =
            body.Velocity;

        // Orbital velocity must be measured relative to
        // the central body:
        //
        // v⃗_rel = v⃗_spacecraft - v⃗_centralBody
        //
        // A shared translational velocity of the whole system
        // must not change the physical meaning of the maneuver.
        var relativeVelocity =
            body.Velocity -
            centralBody.Velocity;

        // For the current Hohmann approximation, the burn occurs
        // where the orbital velocity is assumed to be tangential.
        //
        // Therefore the current relative velocity defines
        // the prograde direction.
        var progradeDirection =
            relativeVelocity.Normalize();

        // Instantaneous impulsive burn:
        //
        // v⃗_new = v⃗_global + Δv * v̂_rel
        body.SetVelocity(
            velocity +
            progradeDirection * deltaV);
    }   
}