using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop
{
    public sealed record EnergyStatistics(
    double Kinetic,
    double Potential,
    double Total);
}
