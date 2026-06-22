using Cosmos.Domain.Structs;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace Cosmos.Desktop;

public sealed class TrailRenderer
{
    private const int CenterX = 640;
    private const int CenterY = 360;

    public void Render(
        Queue<Vector2D> trail,
        Camera camera)
    {
        foreach (var point in trail)
        {
            DrawCircle(
                CenterX +
                (int)((point.X - camera.Position.X)
                * camera.Zoom),

                CenterY +
                (int)((point.Y - camera.Position.Y)
                * camera.Zoom),

                1,

                Color.DarkGray);
        }
    }
}