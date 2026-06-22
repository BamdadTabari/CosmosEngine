using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class UniverseRenderer
{
    private const float RenderScale = 2.5f;

    bool firstTime;

    Vector3 cluster1Center =
    new Vector3(300, 300, 500);

    Vector3 cluster2Center =
        new Vector3(450, 380, 400);

    Vector3 cluster3Center =
        new Vector3(200, 444, 369);

    private readonly Random _random = new();

    private Dictionary<Guid, BodyRenderInfo> _styles =  [];

    private readonly TrailRenderer
    _trailRenderer = new();

    private readonly List<(Vector3 Position, float Radius, Color Color)>
    _stars = [];

    private readonly List<(int x, int y, Color color)>
    _bkstars = [];

    private Vector3 RandomClusterStar(
    Vector3 center,
    float spread)
    {
        return new Vector3(
            center.X + (_random.NextSingle() - 0.5f) * spread,
            center.Y + (_random.NextSingle() - 0.5f) * spread,
            center.Z + (_random.NextSingle() - 0.5f) * spread);
    }

    private void CreateCluster(
    Vector3 center,
    float spread)
    {
        for (int i = 0; i < 1000; i++)
        {
            Color color =
                _random.NextDouble() switch
                {
                    < 0.70 => Color.White,
                    < 0.85 => Color.SkyBlue,
                    < 0.95 => Color.Gold,
                    _ => Color.Orange
                };

            _stars.Add((
                RandomClusterStar(
                    center,
                    spread),

                (float)(_random.NextDouble() * 1.2 + 0.2),

                color));
        }
    }

    //public UniverseRenderer()
    //{
    //    CreateMilkyWayBand();
    //}

    public void DrawStarsBitch()
    {
        foreach (var star in _stars)
        {
            DrawSphere(
                star.Position,
                star.Radius,
                star.Color);
        }
    }
    private void CreateMilkyWayBand()
    {
        for (int i = 0; i < 3000; i++)
        {
            float t =
                (float)(_random.NextDouble() * 12000 - 6000);

            float thickness =
                (float)(_random.NextDouble() * 600 - 300);

            float depth =
                (float)(_random.NextDouble() * 1200 - 600);

            Color color =
                _random.NextDouble() switch
                {
                    < 0.80 => Color.White,
                    < 0.92 => Color.SkyBlue,
                    < 0.98 => Color.Gold,
                    _ => Color.Orange
                };

            _stars.Add((
                new Vector3(
                    t,
                    thickness,
                    depth),

                (float)(_random.NextDouble() * 0.8 + 0.15),

                color));
        }
    }

    public void Render(
        Universe universe,
        Camera camera,
        Dictionary<Guid, Queue<Vector3D>> trails)
    {
        var giant = universe.Bodies.FirstOrDefault(x=> x.Name == "Sun");
        

        BeginDrawing();
        ClearBackground(Color.Black);

        var raylibCamera =
            BuildCamera(camera);

    //    DrawCircle(
    //250,
    //180,
    //180,
    //new Color(60, 20, 90, 40));

    //    DrawCircle(
    //        350,
    //        220,
    //        140,
    //        new Color(90, 40, 140, 25));

    //    DrawCircle(
    //        950,
    //        300,
    //        220,
    //        new Color(20, 60, 140, 30));

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

        //for (int i = 0; i < 10000; i++)
        //{
        //    DrawPixel(
        //        _random.Next(-3000,3000),
        //        _random.Next(-3000, 3000),
        //        _random.NextDouble() switch
        //        {
        //            < 0.70 => Color.White,
        //            < 0.85 => Color.SkyBlue,
        //            < 0.95 => Color.Gold,
        //            _ => Color.Orange
        //        });
        //}

        BeginMode3D(raylibCamera);

        //DrawGrid(20, 10);

        //DrawStarsBitch();

        foreach (var body in universe.Bodies)
        {
            if (!trails.ContainsKey(body.Id))
            {
                trails[body.Id] = new Queue<Vector3D>();
            }

            var trail = trails[body.Id];

            trail.Enqueue(body.Position);

            while (trail.Count > 1000)
            {
                trail.Dequeue();
            }

            _trailRenderer.Render(trail);

            var kirkhar = _styles.ToList();
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
            //var shadedColor = new Color(
            //    (byte)(style.Color.R * depthFactor),
            //    (byte)(style.Color.G * depthFactor),
            //    (byte)(style.Color.B * depthFactor),
            //    (byte)255);

            // 🔵 radius scaling
            float radius = style.Radius;

            // fake lightning (for now of curse dude! dont judge me dude.)
            var dx =
                giant.Position.X -
                body.Position.X;

            var dy =
                giant.Position.Y -
                body.Position.Y;

            var dz =
                giant.Position.Z -
                body.Position.Z;

            var distance =
            Math.Sqrt(
                dx * dx +
                dy * dy +
                dz * dz);

            float lightFactor =
            (float)Math.Clamp(
                1.0 - distance / 1000,
                0.3,
                1.0);

            var st = style;

            var litColor =
            new Color(
                (byte)(style.Color.R * lightFactor),
                (byte)(style.Color.G * lightFactor),
                (byte)(style.Color.B * lightFactor),
                (byte)255);

            if (body == giant)
            {
                DrawSphere(
                    position,
                    radius,
                    Color.Gold);

                continue;
            }

            DrawSphere(position, radius, litColor);
        }

        EndMode3D();
        EndDrawing();
    }

    private float CalculateRadius(Body body)
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


    private Camera3D BuildCamera(Camera camera)
    {
        double targetX = 0;
        double targetY = 0;
        double targetZ = 0;

        if (camera.Target is not null)
        {
            targetX = camera.Target.Position.X * RenderScale;
            targetY = camera.Target.Position.Y * RenderScale;
            targetZ = camera.Target.Position.Z * RenderScale;
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