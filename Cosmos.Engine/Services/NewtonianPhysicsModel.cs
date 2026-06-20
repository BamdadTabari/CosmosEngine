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

                    const double G = 1000;

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
                var newVelocity = new Velocity(
                body.Velocity.X +
                totalAcceleration.X * deltaTime,

                body.Velocity.Y +
                totalAcceleration.Y * deltaTime);

                body.SetVelocity(newVelocity);

                // Update position using velocity:
                // p = p + v * dt
                var newPosition = new Position(
                    body.Position.X +
                    newVelocity.X * deltaTime,

                    body.Position.Y +
                    newVelocity.Y * deltaTime);

                body.SetPosition(newPosition);
            }
        }
    }
}
