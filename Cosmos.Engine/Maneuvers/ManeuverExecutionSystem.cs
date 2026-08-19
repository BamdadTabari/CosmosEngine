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

        // Orbital geometry must be measured relative to
        // the physical central body, not the global origin.
        //
        // r⃗ = x⃗_spacecraft - x⃗_centralBody
        var relativePosition =
            body.Position -
            centralBody.Position;

        var radial =
            relativePosition.Normalize();

        // Current simplified model:
        // rotate the radial direction by +90 degrees
        // in the XY orbital plane to obtain the tangent.
        //
        // This still assumes:
        // - XY-plane orbit
        // - counter-clockwise prograde direction
        //
        // Those assumptions will be reviewed separately.
        var tangent =
            new Vector3D(
                -radial.Y,
                 radial.X,
                 0)
            .Normalize();

        // Instantaneous impulsive burn:
        //
        // v⃗_new = v⃗_old + Δv * t̂
        body.SetVelocity(
            velocity +
            tangent * deltaV);
    }
}