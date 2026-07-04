using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.ValueObjects
{
    public sealed class ReferenceContext
    {
        public ReferenceFrame Frame { get; }

        public Body? Origin { get; }

        public ReferenceContext(
            ReferenceFrame frame,
            Body? origin)
        {
            Frame = frame;
            Origin = origin;
        }
    }
}
