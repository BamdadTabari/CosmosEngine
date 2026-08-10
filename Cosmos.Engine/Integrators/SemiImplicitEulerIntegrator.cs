using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Integrators
{
    public sealed class SemiImplicitEulerIntegrator
    : IIntegrator
    {

        public void Integrate(
            Body body,
            Vector3D acceleration,
            double deltaTime)
        {
            // A simulation timestep must be finite and strictly positive.
            //
            // Zero would perform no meaningful integration, a negative value
            // would imply backward-time integration that this model does not
            // support, and NaN or infinity would corrupt the body's state.
            if (!double.IsFinite(deltaTime) ||
                deltaTime <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaTime),
                    deltaTime,
                    "Delta time must be finite and greater than zero.");
            }


            // Semi-implicit Euler updates velocity before position:
            //
            //     v(t + Δt) = v(t) + a(t) × Δt
            //
            // Acceleration is assumed to remain constant during this
            // individual timestep.
            var velocity =
                body.Velocity +
                acceleration * deltaTime;

            // Position is updated using the newly calculated velocity:
            //
            //     x(t + Δt) = x(t) + v(t + Δt) × Δt
            //
            // This ordering generally behaves better for orbital and other
            // conservative systems than explicit Euler, which would use
            // the old velocity when updating position.
            var position =
                body.Position +
                velocity * deltaTime;

            // Commit the complete state calculated for this timestep.
            //
            // No arbitrary speed or distance limits are applied here.
            // If the result becomes excessively large, diagnostics should
            // expose the instability instead of silently altering physics.
            body.SetVelocity(velocity);
            body.SetPosition(position);
            body.Acceleration = acceleration;
        }
    }
}
