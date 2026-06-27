using Cosmos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Services
{
    public sealed class SphereOfInfluenceCalculator
    {
        public double Calculate(
        Body primary,
        Body secondary)
        {
            var distance =
                secondary.Position.DistanceTo(
                    primary.Position);

            return distance *
                   Math.Pow(
                       secondary.Mass.Value /
                       primary.Mass.Value,
                       2.0 / 5.0);
        }
    }
}
