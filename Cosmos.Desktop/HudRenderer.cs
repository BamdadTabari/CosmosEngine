using static Raylib_cs.Raylib;
using Raylib_cs;

namespace Cosmos.Desktop;

public sealed class HudRenderer
{
    public void Render(
        Camera camera,
        int simulationSpeed,
        bool paused)
    {
        DrawText(
            $"Target: {camera.Target?.Name ?? "None"}",
            20,
            20,
            20,
            Color.White);

        DrawText(
            $"Speed: {simulationSpeed}",
            10,
            40,
            20,
            Color.Green);

        DrawText(
            $"Paused: {paused}",
            10,
            70,
            20,
            Color.Green);

    }
}