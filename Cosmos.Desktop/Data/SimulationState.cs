using Cosmos.Domain.Entities;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop
{
    public sealed class SimulationState
    {
        // Start at real-time simulation speed.
        //
        // Higher time scales require more fixed physics steps per rendered
        // frame. Starting at 100x can overload the main thread before the
        // application has enough time to process input and rendering.
        public int SimulationSpeed { get; set; } = 1;

        public bool Paused { get; set; }

        public Camera.Camera Camera { get; set; } = new();

        public int SelectedBodyIndex { get; set; }

        public ManeuverPlan? CurrentPlan { get; set; }

        public Body? BurnTarget { get; set; }

        public bool BurnExecuted { get; set; }

        public BurnExecutionState BurnState { get; } = new();

        public Body? ControlledBody { get; set; }
    }
}
