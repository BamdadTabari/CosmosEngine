using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop
{
    public sealed record OrbitalStatistics(
    double Distance,
    double Speed,
    double Acceleration,
    double Eccentricity);
}
