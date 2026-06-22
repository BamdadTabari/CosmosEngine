using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class UniverseRenderer
{
    private const float RenderScale = 2.5f;

    private readonly Random _random = new();

    private readonly Dictionary<Guid, BodyRenderInfo> _styles = [];

    public void Render(
        Universe universe,
        Camera camera,
        Dictionary<Guid, Queue<Vector3D>> trails)
    {
        BeginDrawing();
        ClearBackground(Color.Black);

        BeginMode3D(new Camera3D
        {
            Position = new Vector3(300, 150, 300),
            Target = Vector3.Zero,
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        });

        DrawGrid(20, 10);

        foreach (var body in universe.Bodies)
        {
            if (!_styles.ContainsKey(body.Id))
            {
                _styles[body.Id] =
                    new BodyRenderInfo(
                        RandomColor(),
                        CalculateRadius(body));
            }

            var style = _styles[body.Id];

            // 🌌 Scale world position
            var position = new Vector3(
                (float)(body.Position.X * RenderScale),
                (float)(body.Position.Y * RenderScale),
                (float)(body.Position.Z * RenderScale));

            // 🌫 Depth factor (for visual separation)
            float depthFactor =
                Math.Clamp(
                    1f - (position.Z / 1000f),
                    0.3f,
                    1f);

            // 🎨 apply depth shading
            var shadedColor = new Color(
                (byte)(style.Color.R * depthFactor),
                (byte)(style.Color.G * depthFactor),
                (byte)(style.Color.B * depthFactor),
                (byte)255);

            // 🔵 radius scaling
            float radius = (float)Math.Max(
                5,
                Math.Sqrt(body.Mass.Value) / 2);

            DrawSphere(position, radius, shadedColor);
        }

        EndMode3D();
        EndDrawing();
    }

    private float CalculateRadius(Body body)
    {
        return (float)Math.Max(
            5,
            Math.Sqrt(body.Mass.Value) / 2);
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