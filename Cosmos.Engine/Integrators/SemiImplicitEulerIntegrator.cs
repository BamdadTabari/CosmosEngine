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
            var velocity =
                body.Velocity +
                acceleration * deltaTime;

            var position =
                body.Position +
                velocity * deltaTime;

            body.SetVelocity(velocity);
            var speed = body.Velocity.Magnitude();

            const double maxSpeed = 50000;

            if (speed > maxSpeed)
            {
                body.SetVelocity(
                    body.Velocity.Normalize() * maxSpeed);
            }
            body.SetPosition(position);
            var posMag = body.Position.Magnitude();

            const double maxDistance = 5_000_000;

            if (posMag > maxDistance)
            {
                body.SetPosition(
                    body.Position.Normalize() * maxDistance);
            }
            body.Acceleration = acceleration;
        }
    }
}
