using Cosmos.Domain.Structs;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class TrailRenderer
{
    private const float RenderScale = 2.5f;

    public void Render(
        Queue<Vector3D> trail)
    {
        if (trail.Count < 2)
        {
            return;
        }

        var points = trail.ToArray();

        for (int i = 1; i < points.Length; i++)
        {
            var start = new Vector3(
                (float)(points[i - 1].X * RenderScale),
                (float)(points[i - 1].Y * RenderScale),
                (float)(points[i - 1].Z * RenderScale));

            var end = new Vector3(
                (float)(points[i].X * RenderScale),
                (float)(points[i].Y * RenderScale),
                (float)(points[i].Z * RenderScale));

            DrawLine3D(
                start,
                end,
                Color.DarkGray);
        }
    }
}