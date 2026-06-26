using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Engine.Contracts;


namespace Cosmos.Engine.Services
{
    public sealed class NewtonianPhysicsModel
    : IPhysicsModel
    {

        private readonly IIntegrator _integrator;

        Dictionary<Guid, Vector3D> accelerations = [];

        public NewtonianPhysicsModel(IIntegrator integrator)
        {
            _integrator = integrator;
        }

        public void Step(
            Universe universe,
            double deltaTime)
        {

            accelerations.Clear();

            // Phase 1:
            // Calculate accelerations using the universe state
            // at the beginning of the timestep.
            foreach (var body in universe.Bodies)
            {
                Vector3D totalAcceleration = new(0, 0, 0);
                
                accelerations[body.Id] =
                    CalculateAcceleration(universe,body);
            }

            // Phase 2:
            // Integrate all bodies using the accelerations
            // calculated in phase 1.
            foreach (var body in universe.Bodies)
            {
                _integrator.Integrate(
                body,
                accelerations[body.Id],
                deltaTime);
            }
        }

        private Vector3D CalculateAcceleration(Universe universe, Body body) {

            var totalAcceleration = new Vector3D(0, 0, 0);
        
            foreach (var other in universe.Bodies)
            {
                if (body == other) continue;

                var offset = new Vector3D(
                    other.Position.X - body.Position.X,
                    other.Position.Y - body.Position.Y,
                    other.Position.Z - body.Position.Z);

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

            return totalAcceleration;
        }
    }
}
