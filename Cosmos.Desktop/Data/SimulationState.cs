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
        public int SimulationSpeed { get; set; } = 100;

        public bool Paused { get; set; }

        public Camera.Camera Camera { get; set; } = new();

        public int SelectedBodyIndex { get; set; }

        public ManeuverPlan? CurrentPlan { get; set; }

        public Body? BurnTarget { get; set; }

        public bool BurnExecuted { get; set; }
    }
}
