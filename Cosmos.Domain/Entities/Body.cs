using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;

namespace Cosmos.Domain.Entities
{
    public sealed class Body
    {
        public Guid Id { get; private set; }
        public string Name { get; }
        public Vector3D Position { get; private set; }
        public Vector3D Velocity { get; private set; }
        public Mass Mass { get; }
        public BodyType Type { get; }

        public Body(
            Vector3D position,
            Vector3D velocity,
            Mass mass,
            string name,
            BodyType type)
        {
            Id = Guid.NewGuid();
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Name = name;
            Type = type;
        }

        public void SetPosition(Vector3D position)
        {
            Position = position;
        }

        public void SetVelocity(Vector3D velocity)
        {
            Velocity = velocity;
        }

    }
}
