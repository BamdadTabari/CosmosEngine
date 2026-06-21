using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.ValueObjects
{
    public sealed class Camera
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Zoom { get; set; } = 1;
    }
}
