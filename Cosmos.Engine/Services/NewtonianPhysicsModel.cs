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

        // Plummer softening length in normalized simulation length units.
        //
        // Point-mass Newtonian gravity approaches infinity as distance
        // approaches zero. Because bodies currently have no physical radius
        // or collision model, close encounters require a temporary numerical
        // approximation.
        //
        // Softening keeps acceleration finite and continuous without abruptly
        // disabling gravity. It is not a physical body radius and should be
        // reviewed when collision handling is introduced.
        private const double GravitationalSofteningLength =
            0.01;

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

                // Add Plummer softening to prevent the point-mass singularity:
                //
                //     softened r² = r² + ε²
                //
                // At distances much greater than ε, the result approaches ordinary
                // Newtonian gravity. Near zero, acceleration remains finite and
                // changes continuously instead of abruptly becoming zero.
                var softeningSquared =
                    GravitationalSofteningLength *
                    GravitationalSofteningLength;

                var softenedDistanceSquared =
                    distanceSquared +
                    softeningSquared;

                // Vector form of softened gravitational acceleration:
                //
                //              G × M × r⃗
                //     a⃗ = ─────────────────
                //          (r² + ε²)^(3/2)
                //
                // Using the offset vector directly avoids normalizing a zero-length
                // vector and combines direction and magnitude in one calculation.
                var inverseSoftenedDistanceCubed =
                    1.0 /
                    Math.Pow(
                        softenedDistanceSquared,
                        1.5);

                var acceleration =
                    offset *
                    (
                        GravitationalConstant *
                        other.Mass.Value *
                        inverseSoftenedDistanceCubed
                    );

                totalAcceleration +=
                    acceleration;
            }

            return totalAcceleration;
        }
    }
}
