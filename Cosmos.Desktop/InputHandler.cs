using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
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

        HandleZoom(state);

        HandleSimulationSpeed(state);

        HandlePause(state);

        HandleCameraDrag(state);
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
        var wheel =
            GetMouseWheelMove();

        if (wheel != 0)
        {
            state.Camera.Zoom +=
                wheel * 0.1;
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


    private void HandleCameraDrag(SimulationState state)
    {
        if (IsMouseButtonPressed(
            MouseButton.Middle))
        {
            state.Camera.Target = null;
        }


        if (IsMouseButtonDown(
            MouseButton.Middle))
        {
            var delta =
                GetMouseDelta();

            state.Camera.Position =
                new Vector3D(
                    state.Camera.Position.X -
                        delta.X / state.Camera.Zoom,

                    state.Camera.Position.Y -
                        delta.Y / state.Camera.Zoom,

                    0);
                    //state.Camera.Position.Z -
                    //    delta. / state.Camera.Zoom);
        }
    }
}