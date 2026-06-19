using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Numerics;
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
                Vector2D totalAcceleration = new(0, 0);

                foreach(var other in universe.Bodies)
                {
                    if (body == other) continue;

                    var direction = new Vector2D(
                        other.Position.X - body.Position.X,
                        other.Position.Y - body.Position.Y
                        );

                    const double strength = 10;

                    totalAcceleration += new Vector2D(
                        direction.X * strength / body.Mass.Value,
                        direction.Y * strength / body.Mass.Value
                        );
                }

                var newVelocity = new Velocity(
                    body.Velocity.X + 
                    totalAcceleration.X * deltaTime,
                    
                    body.Velocity.Y +
                    totalAcceleration.Y * deltaTime);

                body.SetVelocity(newVelocity);

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
