using Cosmos.Domain.Entities;
using Raylib_cs;
using Cosmos.Domain.Structs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace Cosmos.Desktop;

public sealed class UniverseRenderer
{
    private const int CenterX = 640;
    private const int CenterY = 360;

    private readonly Random _random = new();

    private readonly Dictionary<Guid, BodyRenderInfo>
        _styles = [];

    //private readonly TrailRenderer
    //    _trailRenderer = new();

    public void Render(
    Universe universe,
    Camera camera,
    Dictionary<Guid, Queue<Vector3D>> trails)
    {
        BeginDrawing();
        ClearBackground(Color.Black);

        // فعلاً بدون camera3D واقعی
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
            DrawSphere(
                new Vector3(
                    (float)body.Position.X,
                    (float)body.Position.Y,
                    (float)body.Position.Z),
                (float)Math.Max(5, Math.Sqrt(body.Mass.Value) / 2),
                Color.White);
        }

        EndMode3D();
        EndDrawing();
    }

    private float CalculateRadius(
        Body body)
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