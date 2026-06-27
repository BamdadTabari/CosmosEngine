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

        var _statistics = new OrbitalStatisticsCalculator(orbitalTracker);

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

        var selectedBody =
            camera.Target;

        var stats =
            _statistics.Calculate(selectedBody);

        DrawText(
            $"Target: {selectedBody.Name}",
            20,
            50,
            20,
            Color.Gold);

        DrawText(
            $"Type: {selectedBody.Type}",
            20,
            75,
            20,
            Color.White);

        DrawText(
            $"Mass: {selectedBody.Mass.Value:F2}",
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


        if (selectedBody is not null)
        {
            DrawText(
                $"Periapsis: {orbitalTracker.GetPeriapsis(selectedBody):F2}",
                20,
                205,
                20,
                Color.LightGray);

            DrawText(
                $"Apoapsis: {orbitalTracker.GetApoapsis(selectedBody):F2}",
                20,
                220,
                20,
                Color.LightGray);
        }

        var sun =
            universe.FindBody("Sun");

        if (selectedBody is not null &&
            selectedBody != sun)
        {
            var energy =
                _energyCalculator.Calculate(
                    selectedBody,
                    sun);

            DrawText(
                $"KE: {energy.Kinetic:F0}",
                20,
                260,
                20,
                Color.SkyBlue);

            DrawText(
                $"PE: {energy.Potential:F0}",
                20,
                290,
                20,
                Color.Orange);

            DrawText(
                $"Total: {energy.Total:F0}",
                20,
                320,
                20,
                Color.Green);
        }

        var escapeVelocity =
    _escapeVelocityCalculator.Calculate(
        selectedBody,
        sun);

        DrawText(
            $"Escape Velocity: {escapeVelocity:F2}",
            20,
            350,
            20,
            Color.Yellow);

        var currentSpeed =
    Math.Sqrt(
        selectedBody.Velocity.X *
        selectedBody.Velocity.X +

        selectedBody.Velocity.Y *
        selectedBody.Velocity.Y +

        selectedBody.Velocity.Z *
        selectedBody.Velocity.Z);

        var status =
    currentSpeed >= escapeVelocity
        ? "ESCAPING"
        : "BOUND";

        DrawText(
    $"Orbit Status: {status}",
    20,
    380,
    20,
    status == "ESCAPING"
        ? Color.Red
        : Color.Green);

        var soiCalculator =
    new SphereOfInfluenceStatisticsCalculator();

        var soi =
            soiCalculator.Calculate(
                selectedBody,
                sun);

        DrawText(
    $"SOI Radius: {soi.SphereOfInfluenceRadius:F2}",
    20,
    400,
    20,
    Color.SkyBlue);

        

        DrawText(
            $"Inside SOI: {soi.InsideSphereOfInfluence}",
            20,
            420,
            20,
            Color.Green);


        if (state.CurrentPlan is not null)
        {
            DrawText(
                "Transfer Plan",
                20,
                440,
                20,
                Color.Orange);

            DrawText(
                $"ΔV1: {state.CurrentPlan.DeltaV1:F2}",
                20,
                460,
                20,
                Color.White);

            DrawText(
                $"ΔV2: {state.CurrentPlan.DeltaV2:F2}",
                20,
                480,
                20,
                Color.White);

            DrawText(
                $"Total ΔV: {state.CurrentPlan.TotalDeltaV:F2}",
                20,
                500,
                20,
                Color.Yellow);

            DrawText(
                $"Transfer Time: {state.CurrentPlan.TransferTime:F2}",
                20,
                540,
                20,
                Color.SkyBlue);
        }
    }
}

