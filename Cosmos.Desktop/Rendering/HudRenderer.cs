using Cosmos.Domain.Entities;
using Cosmos.Engine.Calculators;
using Cosmos.Engine.Tracking;
using Raylib_cs;
using System.Diagnostics;
using Cosmos.Desktop.Camera;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop.Rendering;

public sealed class HudRenderer
{
    private readonly OrbitalEnergyCalculator
    _energyCalculator =
        new();

    private readonly EscapeVelocityCalculator
    _escapeVelocityCalculator =
        new();


    public void Render(
     Universe universe,
     Camera.Camera camera,
     int simulationSpeed,
     OrbitalTracker orbitalTracker,
     SimulationState state)
    {
        int y = 20;

        void DrawHudLine(
            string text,
            Color color)
        {
            DrawText(
                text,
                20,
                y,
                20,
                color);

            y += 22;
        }

        DrawRectangle(
            10,
            10,
            350,
            650,
            new Color(0, 0, 0, 180));

        DrawHudLine(
            $"Bodies: {universe.Bodies.Count}",
            Color.White);

        if (camera.Target is null)
        {
            DrawHudLine(
                "Target: None",
                Color.Gray);

            return;
        }

        var statistics =
            new OrbitalStatisticsCalculator(
                orbitalTracker);

        var selectedBody =
            camera.Target;

        var stats =
            statistics.Calculate(
                selectedBody);

        DrawHudLine(
            $"Controlled Body: {state.ControlledBody?.Name}",
            Color.Green);

        DrawHudLine(
            $"Target: {selectedBody.Name}",
            Color.Gold);

        DrawHudLine(
            $"Type: {selectedBody.Type}",
            Color.White);

        DrawHudLine(
            $"Mass: {selectedBody.Mass.Value:F2}",
            Color.White);

        DrawHudLine(
            $"Velocity: {stats.Speed:F1}",
            Color.White);

        DrawHudLine(
            $"Distance: {stats.Distance:F1}",
            Color.White);

        DrawHudLine(
            $"Simulation Speed: {simulationSpeed}",
            Color.White);

        DrawHudLine(
            $"Acceleration: {stats.Acceleration:F2}",
            Color.White);

        DrawHudLine(
            $"Periapsis: {orbitalTracker.GetPeriapsis(selectedBody):F2}",
            Color.LightGray);

        DrawHudLine(
            $"Apoapsis: {orbitalTracker.GetApoapsis(selectedBody):F2}",
            Color.LightGray);

        var sun =
            universe.FindBody("Sun");

        if (selectedBody != sun)
        {
            var energy =
                _energyCalculator.Calculate(
                    selectedBody,
                    sun);

            DrawHudLine(
                $"KE: {energy.Kinetic:F0}",
                Color.SkyBlue);

            DrawHudLine(
                $"PE: {energy.Potential:F0}",
                Color.Orange);

            DrawHudLine(
                $"Total: {energy.Total:F0}",
                Color.Green);
        }

        var escapeVelocity =
            _escapeVelocityCalculator.Calculate(
                selectedBody,
                sun);

        DrawHudLine(
            $"Escape Velocity: {escapeVelocity:F2}",
            Color.Yellow);

        var currentSpeed =
            selectedBody.Velocity.Magnitude();

        var orbitStatus =
            currentSpeed >= escapeVelocity
                ? "ESCAPING"
                : "BOUND";

        DrawHudLine(
            $"Orbit Status: {orbitStatus}",
            orbitStatus == "ESCAPING"
                ? Color.Red
                : Color.Green);

        var soiCalculator =
            new SphereOfInfluenceStatisticsCalculator();

        var soi =
            soiCalculator.Calculate(
                selectedBody,
                sun);

        DrawHudLine(
            $"SOI Radius: {soi.SphereOfInfluenceRadius:F2}",
            Color.SkyBlue);

        DrawHudLine(
            $"Inside SOI: {soi.InsideSphereOfInfluence}",
            Color.Green);

        if (state.CurrentPlan is not null)
        {
            y += 10;

            DrawHudLine(
                "TRANSFER PLAN",
                Color.Orange);

            DrawHudLine(
                $"ΔV1: {state.CurrentPlan.DeltaV1:F2}",
                Color.White);

            DrawHudLine(
                $"ΔV2: {state.CurrentPlan.DeltaV2:F2}",
                Color.White);

            DrawHudLine(
                $"Total ΔV: {state.CurrentPlan.TotalDeltaV:F2}",
                Color.Yellow);

            DrawHudLine(
                $"Transfer Time: {state.CurrentPlan.TransferTime:F2}",
                Color.SkyBlue);
        }
    }
}

