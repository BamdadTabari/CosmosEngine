using Cosmos.Domain.Entities;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace Cosmos.Desktop;

public sealed class HudRenderer
{

    private readonly OrbitalStatisticsCalculator
    _statistics = new();

    public void Render(
        Universe universe,
        Camera camera,
        int simulationSpeed)
    {
        DrawRectangle(
            10,
            10,
            300,
            140,
            new Color(0, 0, 0, 180));

        DrawText(
            $"Bodies: {universe.Bodies.Count}",
            20,
            20,
            20,
            Color.White);

        if (camera.Target is null)
        {
            DrawText(
                "Target: None",
                20,
                50,
                20,
                Color.Gray);

            return;
        }

        var body =
            camera.Target;

        var stats =
            _statistics.Calculate(body);

        DrawText(
            $"Target: {body.Name}",
            20,
            50,
            20,
            Color.Gold);

        DrawText(
            $"Type: {body.Type}",
            20,
            75,
            20,
            Color.White);

        DrawText(
            $"Mass: {body.Mass.Value:F2}",
            20,
            100,
            20,
            Color.White);

        DrawText(
            $"Velocity: {stats.Speed:F1}",
            20,
            125,
            20,
            Color.White);

        DrawText(
            $"Distance: {stats.Distance:F1}",
            20,
            150,
            20,
            Color.White);

        DrawText(
            $"Simulation Speed: {simulationSpeed}",
            20,
            175,
            20,
            Color.White);
        DrawText(
            $"Target Acceleration: {stats.Acceleration}",
            20,
            190,
            20,
            Color.White);
    }
}

