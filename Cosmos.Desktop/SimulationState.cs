using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop
{
    public sealed class SimulationState
    {
        public int SimulationSpeed { get; set; }

        public bool Paused { get; set; }

        public Camera Camera { get; set; } = new();

        public int SelectedBodyIndex { get; set; }
    }
}
