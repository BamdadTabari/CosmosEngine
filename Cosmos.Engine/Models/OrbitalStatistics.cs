using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Models
{
    public sealed record OrbitalStatistics(
    double Distance,
    double Speed,
    double Acceleration,
    double Eccentricity,
    double Periapsis,
    double Apoapsis);
}
