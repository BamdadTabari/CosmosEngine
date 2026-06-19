using Cosmos.Domain.Entities;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Services
{
    public sealed class NewtonianPhysicsModel
    : IPhysicsModel
    {
        public void Step(
            Universe universe,
            double deltaTime)
        {
            foreach (var body in universe.Bodies)
            {
                var newVelocity = new Velocity(
                    body.Velocity.X + 
                    body.Acceleration.X * deltaTime,
                    
                    body.Velocity.Y +
                    body.Acceleration.Y * deltaTime);

                var newPosition = new Position(
                    body.Position.X +
                    body.Velocity.X * deltaTime,

                    body.Position.Y +
                    body.Velocity.Y * deltaTime);

                body.SetPosition(newPosition);
            }
        }
    }
}
