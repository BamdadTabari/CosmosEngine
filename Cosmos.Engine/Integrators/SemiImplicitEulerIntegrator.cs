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

        // Emergency numerical guard expressed in the current normalized
        // simulation-unit system.
        //
        // This value is not a physical speed limit. It exists to prevent
        // an unstable timestep or an extremely close gravitational
        // encounter from producing an impractically large stored velocity.
        //
        // A normal Earth-like orbital velocity in the current data is
        // approximately 260 simulation length units per time unit, making
        // this limit far outside ordinary simulation behavior.
        private const double MaximumSpeed =
            50_000;

        // Emergency spatial guard expressed in simulation length units.
        //
        // This is not a physical boundary of the simulated universe.
        // When a body crosses this radius, its position is projected back
        // onto a sphere centered at the coordinate origin.
        //
        // Velocity is not changed, so this behavior is neither a collision
        // response nor a physically meaningful boundary condition.
        private const double MaximumDistanceFromOrigin =
            5_000_000;

        public void Integrate(
            Body body,
            Vector3D acceleration,
            double deltaTime)
        {


            var velocity =
                body.Velocity +
                acceleration * deltaTime;

            var position =
                body.Position +
                velocity * deltaTime;

            body.SetVelocity(velocity);
            var speed = body.Velocity.Magnitude();

            if (speed > MaximumSpeed)
            {
                // Preserve the velocity direction while reducing its magnitude
                // to the current emergency limit.
                //
                // Notice that position has already been calculated using the
                // original, unclamped velocity. The characterization tests
                // intentionally document this ordering for the upcoming review.
                body.SetVelocity(
                    body.Velocity.Normalize() *
                    MaximumSpeed);
            }

            body.SetPosition(position);
            var posMag = body.Position.Magnitude();

            if (posMag > MaximumDistanceFromOrigin)
            {
                // Project the body radially back onto the artificial boundary.
                //
                // This changes position without changing velocity or applying
                // a force, which makes the operation a numerical containment
                // mechanism rather than a physical interaction.
                body.SetPosition(
                    body.Position.Normalize() *
                    MaximumDistanceFromOrigin);
            }
            body.Acceleration = acceleration;
        }
    }
}
