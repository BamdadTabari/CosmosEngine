using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Models
{
    public sealed record EnergyStatistics(
    double Kinetic,
    double Potential,
    double Total);
}
