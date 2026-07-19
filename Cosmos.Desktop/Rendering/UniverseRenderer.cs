using Cosmos.Desktop.Camera;
using Cosmos.Desktop.Rendering;
using Cosmos.Desktop.Styles;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public sealed class UniverseRenderer
{
    private const float RenderScale = 2.5f;

    private readonly TrailRenderer
    _trailRenderer = new();

    private readonly PlanetStyleProvider
    _styleProvider;


    private Texture2D _starsTexture;
    private Texture2D _nebulaBlue;
    private Texture2D _nebulaPurple;
    private Texture2D _sky;

    public UniverseRenderer(
        PlanetStyleProvider styleProvider)
    {
        _styleProvider =
            styleProvider;
        _starsTexture = LoadTexture("Assets/Sky/stars.png");
        _nebulaBlue = LoadTexture("Assets/Sky/nebula_blue.png");
        _nebulaPurple = LoadTexture("Assets/Sky/nebula_purple.png");
        _sky = LoadTexture("Assets/Sky/sky.png");

    }

    public void Render(
        Universe universe,
        Camera camera,
        Dictionary<Guid, Queue<Vector3D>> trails)
    {

        BeginDrawing();

        float skyOffsetX =
            (float)(camera.AngleX * 400.0);

        float skyOffsetY =
            (float)(camera.AngleY * 120.0);


        skyOffsetX %= _sky.Width;

        skyOffsetY %= _sky.Height;

        ClearBackground(new Color(2, 3, 8, 255));


        DrawTexturePro(
          _sky,
          new Rectangle(
            skyOffsetX,
            skyOffsetY,
            _sky.Width,
            _sky.Height),
          new Rectangle(
              0,
              0,
              GetScreenWidth(),
              GetScreenHeight()),
          Vector2.Zero,
          0,
          new Color(170, 170, 170, 255));


        DrawTextureEx(
            _nebulaBlue!,
            new Vector2(
                -150 - skyOffsetX * 0.15f,
                -80 - skyOffsetY * 0.15f),
            0,
            1.4f,
            new Color(255, 255, 255, 90));

        DrawTextureEx(
            _nebulaPurple!,
                    new Vector2(
                        900 - skyOffsetX * 0.25f,
                        250 - skyOffsetY * 0.25f),
                    0,
                    1.2f,
                    new Color(255, 255, 255, 70));

        DrawTextureEx(
            _starsTexture!,
            new Vector2(
            -skyOffsetX * 0.05f,
            -skyOffsetY * 0.05f),
            0,
            1f,
            new Color(255, 255, 255, 120));



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
        var style =
            _styleProvider.GetStyle(body);

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

        // dumb and dumber 
        if (body.Type == BodyType.Spacecraft)
        {
            DrawCube(
                position,
                10,
                10,
                10,
                Color.Red);

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