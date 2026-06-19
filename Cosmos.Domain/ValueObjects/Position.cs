using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.ValueObjects
{
    public sealed record Position(
    double X,
    double Y);
}
