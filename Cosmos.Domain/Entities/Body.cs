using Cosmos.Domain.ValueObjects;

namespace Cosmos.Domain.Entities
{
    public sealed class Body
    {
        public Guid Id { get; private set; }
        public Position Position { get; private set; }
        public Velocity Velocity { get; private set; }
        public Mass Mass { get; }
        public Acceleration Acceleration { get; private set; }

        public Body(
            Position position,
            Velocity velocity,
            Acceleration acceleration,
            Mass mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Acceleration = acceleration;
        }

        public void SetPosition(Position position)
        {
            Position = position;
        }

        public void SetVelocity(Velocity velocity)
        {
            Velocity = velocity;
        }

        public void SetAcceleration(Acceleration acceleration)
        {
            Acceleration = acceleration;
        }
    }
}
