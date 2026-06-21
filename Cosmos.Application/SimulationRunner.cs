using Cosmos.Application.Renderers;
using Cosmos.Domain.Entities;
using Cosmos.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Application
{
    public sealed class SimulationRunner
    {
        public void Run(
            Universe universe,
            IPhysicsModel physics,
            ConsoleUniverseRenderer renderer)
        {
            Console.CursorVisible = false;

            while (true)
            {
                physics.Step(universe, 0.1);

                renderer.Render(universe);

                Thread.Sleep(100);
            }
        }
    }
}
