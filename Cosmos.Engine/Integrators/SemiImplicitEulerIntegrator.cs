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
            body.SetPosition(position);
            body.Acceleration = acceleration;
        }
    }
}
