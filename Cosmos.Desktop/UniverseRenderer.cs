using Cosmos.Domain.Entities;
using Raylib_cs;
using Cosmos.Domain.Structs;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class UniverseRenderer
{
    private const int CenterX = 640;
    private const int CenterY = 360;

    private readonly Random _random = new();

    private readonly Dictionary<Guid, BodyRenderInfo>
        _styles = [];

    private readonly TrailRenderer
        _trailRenderer = new();

    public void Render(
        Universe universe,
        Camera camera,
        Dictionary<Guid, Queue<Vector2D>> trails)
    {
        foreach (var body in universe.Bodies)
        {
            if (!_styles.ContainsKey(body.Id))
            {
                _styles[body.Id] =
                    new BodyRenderInfo(
                        RandomColor(),
                        CalculateRadius(body));
            }

            var style =
                _styles[body.Id];

            if (!trails.ContainsKey(body.Id))
            {
                trails[body.Id] =
                    new Queue<Vector2D>();
            }

            var trail =
                trails[body.Id];

            trail.Enqueue(body.Position);

            while (trail.Count > 1000)
            {
                trail.Dequeue();
            }

            _trailRenderer.Render(
                trail,
                camera);

            DrawCircle(
                CenterX +
                (int)((body.Position.X - camera.Position.X)
                * camera.Zoom),

                CenterY +
                (int)((body.Position.Y - camera.Position.Y)
                * camera.Zoom),

                style.Radius,

                style.Color);
        }
    }

    private float CalculateRadius(
        Body body)
    {
        return (float)Math.Max(
            3,
            Math.Sqrt(body.Mass.Value) / 5);
    }

    private Color RandomColor()
    {
        return new Color(
            _random.Next(50, 255),
            _random.Next(50, 255),
            _random.Next(50, 255),
            255);
    }
}