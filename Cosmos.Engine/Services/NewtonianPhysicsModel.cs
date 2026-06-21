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
        // TODO:
        // Current implementation is O(N²).
        // Optimize with spatial partitioning if simulation size grows.
        // In other word, CPU will get fucked if you forget this TODO
        public void Step(
            Universe universe,
            double deltaTime)
        {
            Dictionary<Guid, Vector2D> newVelocities = new();

            foreach (var body in universe.Bodies)
            {
                Vector2D totalAcceleration = new(0, 0);
                
                foreach(var other in universe.Bodies)
                {
                    if (body == other) continue;

                    var offset = new Vector2D(
                        other.Position.X - body.Position.X,
                        other.Position.Y - body.Position.Y);

                    // Distance squared between two bodies (r²)
                    var distanceSquared =
                        offset.LengthSquared();

                    if (distanceSquared < 0.0001)
                    {
                        continue;
                    }

                    // Unit vector pointing from current body to the other body
                    var direction =
                        offset.Normalize();

                    const double G = 100;

                    // Newton's law of universal gravitation:
                    // F = G * m1 * m2 / r²
                    var force =
                    G *
                    body.Mass.Value *
                    other.Mass.Value /
                    distanceSquared;

                    // Newton's second law:
                    // F = m * a
                    // a = F / m
                    var accelerationMagnitude =
                        force / body.Mass.Value;

                    totalAcceleration +=
                        direction * accelerationMagnitude;
                }

                // Update velocity using acceleration:
                // v = v + a * dt
                var newVelocity = new Vector2D(
                body.Velocity.X +
                totalAcceleration.X * deltaTime,

                body.Velocity.Y +
                totalAcceleration.Y * deltaTime);

                newVelocities.Add(body.Id, newVelocity);
            }

            foreach (var velocity in newVelocities)
            {
                var body = universe.Bodies.SingleOrDefault(x => x.Id == velocity.Key);

                if (body == null) throw new Exception("your universe fucked");

                body.SetVelocity(velocity.Value);

                // Update position using velocity:
                // p = p + v * dt
                var newPosition = new Vector2D(
                    body.Position.X +
                    velocity.Value.X * deltaTime,

                    body.Position.Y +
                    velocity.Value.Y * deltaTime);

                body.SetPosition(newPosition);
            }
        }
    }
}
