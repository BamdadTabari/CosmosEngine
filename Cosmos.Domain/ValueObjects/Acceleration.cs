using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.ValueObjects
{
    public sealed record Acceleration(
        double X,
        double Y);
}
