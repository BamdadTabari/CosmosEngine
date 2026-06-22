using Cosmos.Domain.Entities;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cosmos.Desktop;

public sealed class InputHandler
{
    public void Handle(
        SimulationState state,
        Body giant,
        Body alpha,
        Body beta,
        Body gamma,
        Body delta,
        Body epsilon)
    {
        HandleCameraTarget(
            state,
            giant,
            alpha,
            beta,
            gamma,
            delta,
            epsilon);

        HandleOrbitCamera(state);

        HandleSimulationSpeed(state);

        HandlePause(state);
    }

    private void HandleCameraTarget(
        SimulationState state,
        Body giant,
        Body alpha,
        Body beta,
        Body gamma,
        Body delta,
        Body epsilon)
    {
        if (IsKeyPressed(KeyboardKey.One))
            state.Camera.Target = giant;

        if (IsKeyPressed(KeyboardKey.Two))
            state.Camera.Target = alpha;

        if (IsKeyPressed(KeyboardKey.Three))
            state.Camera.Target = beta;

        if (IsKeyPressed(KeyboardKey.Four))
            state.Camera.Target = gamma;

        if (IsKeyPressed(KeyboardKey.Five))
            state.Camera.Target = delta;

        if (IsKeyPressed(KeyboardKey.Six))
            state.Camera.Target = epsilon;
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