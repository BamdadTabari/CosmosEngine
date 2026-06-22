using Cosmos.Domain.Entities;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class InputHandler
{
    public void Handle(
        SimulationState state,
        Body giant,
        Body small1,
        Body small2,
        Body small3)
    {
        HandleCameraTarget(
            state,
            giant,
            small1,
            small2,
            small3);

        HandleOrbitCamera(state);

        HandleSimulationSpeed(state);

        HandlePause(state);
    }

    private void HandleCameraTarget(
        SimulationState state,
        Body giant,
        Body small1,
        Body small2,
        Body small3)
    {
        if (IsKeyPressed(KeyboardKey.One))
            state.Camera.Target = giant;

        if (IsKeyPressed(KeyboardKey.Two))
            state.Camera.Target = small1;

        if (IsKeyPressed(KeyboardKey.Three))
            state.Camera.Target = small2;

        if (IsKeyPressed(KeyboardKey.Four))
            state.Camera.Target = small3;
    }

    private void HandleOrbitCamera(
        SimulationState state)
    {
        var wheel = GetMouseWheelMove();

        state.Camera.Distance -= wheel * 20;

        state.Camera.Distance =
            Math.Clamp(
                state.Camera.Distance,
                50,
                2000);

        if (IsMouseButtonDown(MouseButton.Middle))
        {
            var delta = GetMouseDelta();

            state.Camera.AngleX += delta.X * 0.01;
            state.Camera.AngleY += delta.Y * 0.01;

            state.Camera.AngleY =
                Math.Clamp(
                    state.Camera.AngleY,
                    -1.5,
                    1.5);
        }
    }

    private void HandleSimulationSpeed(
        SimulationState state)
    {
        if (IsKeyPressed(KeyboardKey.Up))
        {
            state.SimulationSpeed += 10;
        }

        if (IsKeyPressed(KeyboardKey.Down))
        {
            state.SimulationSpeed -= 10;
        }

        state.SimulationSpeed =
            Math.Max(
                1,
                state.SimulationSpeed);
    }

    private void HandlePause(
        SimulationState state)
    {
        if (IsKeyPressed(KeyboardKey.Space))
        {
            state.Paused =
                !state.Paused;
        }
    }
}