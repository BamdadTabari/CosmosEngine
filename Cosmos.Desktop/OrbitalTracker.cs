using Cosmos.Domain.Entities;

namespace Cosmos.Desktop;

public sealed class OrbitalTracker
{
    private readonly Dictionary<Guid, double>
        _periapsis = [];

    private readonly Dictionary<Guid, double>
        _apoapsis = [];

    public void Update(
        IEnumerable<Body> bodies)
    {
        foreach (var body in bodies)
        {
            var distance =
                Math.Sqrt(
                    body.Position.X * body.Position.X +
                    body.Position.Y * body.Position.Y +
                    body.Position.Z * body.Position.Z);

            if (!_periapsis.ContainsKey(body.Id))
            {
                _periapsis[body.Id] = distance;
                _apoapsis[body.Id] = distance;

                continue;
            }

            if (distance < _periapsis[body.Id])
            {
                _periapsis[body.Id] = distance;
            }

            if (distance > _apoapsis[body.Id])
            {
                _apoapsis[body.Id] = distance;
            }
        }
    }

    public double GetPeriapsis(
        Body body)
    {
        return _periapsis.GetValueOrDefault(body.Id);
    }

    public double GetApoapsis(
        Body body)
    {
        return _apoapsis.GetValueOrDefault(body.Id);
    }
}