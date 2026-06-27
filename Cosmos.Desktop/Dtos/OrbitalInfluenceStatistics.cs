using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop.Dtos
{
    public sealed record OrbitalInfluenceStatistics(
    string ParentBody,
    double SphereOfInfluenceRadius,
    bool InsideSphereOfInfluence);
}
