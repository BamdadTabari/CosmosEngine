using Cosmos.Domain.Entities;

namespace Cosmos.Engine.Tracking;

public sealed class OrbitalTracker
{
    private readonly Dictionary<Guid, double> _periapsis = [];
    private readonly Dictionary<Guid, double> _apoapsis = [];

    public void Update(
        IEnumerable<Body> bodies)
    {
        var sun =
            bodies.FirstOrDefault(
                x => x.Name == "Sun");

        if (sun is null)
        {
            return;
        }

        foreach (var body in bodies)
        {
            if (body == sun)
            {
                continue;
            }

            var distance =
                CalculateDistance(
                    body,
                    sun);

            if (!_periapsis.ContainsKey(body.Id))
            {
                _periapsis[body.Id] = distance;
            }

            if (!_apoapsis.ContainsKey(body.Id))
            {
                _apoapsis[body.Id] = distance;
            }

            _periapsis[body.Id] =
                Math.Min(
                    _periapsis[body.Id],
                    distance);

            _apoapsis[body.Id] =
                Math.Max(
                    _apoapsis[body.Id],
                    distance);
        }
    }

    public double GetPeriapsis(
        Body body)
    {
        return _periapsis.GetValueOrDefault(
            body.Id,
            0);
    }

    public double GetApoapsis(
        Body body)
    {
        return _apoapsis.GetValueOrDefault(
            body.Id,
            0);
    }

    private static double CalculateDistance(
        Body a,
        Body b)
    {
        var dx =
            a.Position.X - b.Position.X;

        var dy =
            a.Position.Y - b.Position.Y;

        var dz =
            a.Position.Z - b.Position.Z;

        return Math.Sqrt(
            dx * dx +
            dy * dy +
            dz * dz);
    }
}