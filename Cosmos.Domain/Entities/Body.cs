using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;

namespace Cosmos.Domain.Entities
{
    public sealed class Body
    {
        public Guid Id { get; private set; }
        public string Name { get; }
        public Vector2D Position { get; private set; }
        public Vector2D Velocity { get; private set; }
        public Mass Mass { get; }

        public Body(
            Vector2D position,
            Vector2D velocity,
            Mass mass,
            string name)
        {
            Id = Guid.NewGuid();
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Name = name;
        }

        public void SetPosition(Vector2D position)
        {
            Position = position;
        }

        public void SetVelocity(Vector2D velocity)
        {
            Velocity = velocity;
        }

    }
}
