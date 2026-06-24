using Cosmos.Desktop;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public sealed class UniverseRenderer
{
    private const float RenderScale = 2.5f;

    private readonly Random _random = new();

    private readonly Dictionary<Guid, BodyRenderInfo>
        _styles = [];

    private readonly TrailRenderer
        _trailRenderer = new();

    public void Render(
        Universe universe,
        Camera camera,
        Dictionary<Guid, Queue<Vector3D>> trails)
    {
        BeginDrawing();
        ClearBackground(Color.Black);

        DrawNebulaBackground();

        BeginMode3D(
            BuildCamera(camera));

        foreach (var body in universe.Bodies)
        {
            UpdateTrail(
                body,
                trails);

            _trailRenderer.Render(
                trails[body.Id]);

            DrawBody(
                body,
                universe);
        }

        EndMode3D();
        EndDrawing();
    }

    private void UpdateTrail(
        Body body,
        Dictionary<Guid, Queue<Vector3D>> trails)
    {
        if (!trails.ContainsKey(body.Id))
        {
            trails[body.Id] =
                new Queue<Vector3D>();
        }

        trails[body.Id]
            .Enqueue(body.Position);

        while (trails[body.Id].Count > 1000)
        {
            trails[body.Id]
                .Dequeue();
        }
    }

    private void DrawBody(
        Body body,
        Universe universe)
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

        var position =
            new Vector3(
                (float)(body.Position.X * RenderScale),
                (float)(body.Position.Y * RenderScale),
                (float)(body.Position.Z * RenderScale));

        var radius =
            style.Radius;

        if (body.Type == BodyType.Star)
        {
            DrawSphere(
                position,
                radius,
                Color.Gold);

            return;
        }

        DrawSphere(
            position,
            radius,
            style.Color);
    }

    private void DrawNebulaBackground()
    {
        DrawCircle(
            300,
            200,
            240,
            new Color(80, 30, 120, 20));

        DrawCircle(
            420,
            260,
            180,
            new Color(120, 60, 180, 15));

        DrawCircle(
            900,
            250,
            320,
            new Color(30, 70, 140, 18));

        DrawCircle(
            1050,
            320,
            240,
            new Color(20, 40, 100, 15));
    }

    private float CalculateRadius(
        Body body)
    {
        return (float)Math.Clamp(
            Math.Pow(body.Mass.Value, 0.25),
            2,
            25);
    }

    private Color RandomColor()
    {
        return new Color(
            _random.Next(50, 255),
            _random.Next(50, 255),
            _random.Next(50, 255),
            255);
    }

    private Camera3D BuildCamera(
        Camera camera)
    {
        double targetX = 0;
        double targetY = 0;
        double targetZ = 0;

        if (camera.Target is not null)
        {
            targetX =
                camera.Target.Position.X *
                RenderScale;

            targetY =
                camera.Target.Position.Y *
                RenderScale;

            targetZ =
                camera.Target.Position.Z *
                RenderScale;
        }

        var x =
            targetX +
            Math.Cos(camera.AngleX)
            * camera.Distance;

        var z =
            targetZ +
            Math.Sin(camera.AngleX)
            * camera.Distance;

        var y =
            targetY +
            Math.Sin(camera.AngleY)
            * camera.Distance;

        return new Camera3D
        {
            Position = new Vector3(
                (float)x,
                (float)y,
                (float)z),

            Target = new Vector3(
                (float)targetX,
                (float)targetY,
                (float)targetZ),

            Up = Vector3.UnitY,

            FovY = 45,

            Projection =
                CameraProjection.Perspective
        };
    }
}