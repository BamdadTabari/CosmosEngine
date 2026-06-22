using Cosmos.Domain.Entities;
using static Raylib_cs.Raylib;
using Raylib_cs;

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

        HandleZoom(state);

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
        {
            state.Camera.Target = giant;
        }

        if (IsKeyPressed(KeyboardKey.Two))
        {
            state.Camera.Target = small1;
        }

        if (IsKeyPressed(KeyboardKey.Three))
        {
            state.Camera.Target = small2;
        }

        if (IsKeyPressed(KeyboardKey.Four))
        {
            state.Camera.Target = small3;
        }
    }

    private void HandleZoom(
        SimulationState state)
    {
        if (IsKeyDown(KeyboardKey.Equal))
        {
            state.Camera.Zoom += 0.01;
        }

        if (IsKeyDown(KeyboardKey.Minus))
        {
            state.Camera.Zoom -= 0.01;
        }

        state.Camera.Zoom =
            Math.Max(
                0.1,
                state.Camera.Zoom);
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