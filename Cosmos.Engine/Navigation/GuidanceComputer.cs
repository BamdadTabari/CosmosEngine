using Cosmos.Domain.Entities;
using Cosmos.Domain.Navigation;
using Cosmos.Engine.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Navigation
{
    public sealed class GuidanceComputer
    {
        private readonly OrbitAnalyzer _orbitAnalyzer;

        public NavigationSolution Solve(
            Universe universe,
            Spacecraft spacecraft,
            INavigationTarget target)
        {
            var orbit =
                _orbitAnalyzer.Analyze(
                    universe,
                    spacecraft);

                        var dx =
                target.Position.X -
                spacecraft.Position.X;

            var dy =
                target.Position.Y -
                spacecraft.Position.Y;

            var dz =
                target.Position.Z -
                spacecraft.Position.Z;

            var distance =
                Math.Sqrt(
                    dx * dx +
                    dy * dy +
                    dz * dz);

            var dvx =
                target.Velocity.X -
                spacecraft.Velocity.X;

            var dvy =
                target.Velocity.Y -
                spacecraft.Velocity.Y;

            var dvz =
                target.Velocity.Z -
                spacecraft.Velocity.Z;

            var relativeSpeed =
                Math.Sqrt(
                    dvx * dvx +
                    dvy * dvy +
                    dvz * dvz);

            return new NavigationSolution(
                orbit,
                target,
                distance,
                relativeSpeed);
        }
    }
}
