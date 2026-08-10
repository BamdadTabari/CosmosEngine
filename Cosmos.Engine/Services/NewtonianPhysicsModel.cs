using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Engine.Contracts;


namespace Cosmos.Engine.Services
{
    public sealed class NewtonianPhysicsModel
    : IPhysicsModel
    {

        // Cosmos Engine currently uses a normalized simulation-unit system,
        // not SI units.
        //
        // In this unit system:
        // - position is measured in simulation length units;
        // - mass is measured in simulation mass units;
        // - time is measured in simulation time units;
        // - velocity is length unit / time unit;
        // - acceleration is length unit / time unit².
        //
        // Therefore, this constant has the dimensions:
        //
        //     length³ / (mass × time²)
        //
        // Its value is calibrated to the scaled data in solar-system.json.
        // For example, a central mass of 100,000 and an orbital radius
        // of 150 produce a circular velocity close to Earth's value of 260.
        private const double GravitationalConstant = 100;

        // Interactions closer than 0.01 simulation length units are
        // currently ignored:
        //
        //     minimum distance² = 0.01² = 0.0001
        //
        // This is a numerical safety guard, not a physical collision model
        // and not gravitational softening. It creates a discontinuity:
        // gravity is calculated immediately outside the threshold but
        // becomes exactly zero inside it.
        private const double MinimumInteractionDistanceSquared =
            0.0001;

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
            // Calculate every body's acceleration from the shared universe
            // state at the beginning of the timestep.
            //
            // No body is moved during this phase. This prevents an earlier body
            // in the collection from affecting the acceleration calculation of
            // a later body using a newer state.
            foreach (var body in universe.Bodies)
            {                
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

                if (distanceSquared <
                        MinimumInteractionDistanceSquared)
                {
                    // Ignore extremely close interactions to prevent division by
                    // zero or an unbounded acceleration.
                    //
                    // This is only a temporary numerical guard. A future model
                    // should handle close encounters explicitly through collision
                    // handling, gravitational softening, or both.
                    continue;
                }

                // Unit vector pointing from current body to the other body
                var direction =
                    offset.Normalize();


                // Gravitational acceleration caused by the other body:
                //
                //     F = G × m₁ × m₂ / r²
                //     a₁ = F / m₁
                //
                // Substituting force into Newton's second law:
                //
                //     a₁ = G × m₂ / r²
                //
                // The mass of the body being accelerated cancels out.
                // Therefore, acceleration depends on the attracting body's mass,
                // not on the current body's own mass.
                var accelerationMagnitude =
                    GravitationalConstant *
                    other.Mass.Value /
                    distanceSquared;

                // Convert the scalar acceleration magnitude into a vector pointing
                // from the current body toward the attracting body, then add it to
                // the acceleration produced by all previous bodies.
                totalAcceleration +=
                    direction *
                    accelerationMagnitude;
            }

            return totalAcceleration;
        }
    }
}
