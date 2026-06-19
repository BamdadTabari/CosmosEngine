using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.ValueObjects
{
    public sealed record Mass
    {
        public double Value { get; }

        public Mass(double value)
        {
            if (value <= 0)
                throw new ArgumentException("mass must be greather than zero");

            if(double.IsInfinity(value))
                throw new ArgumentException("mass can not be infinity");

            if (double.IsNaN(value))
                throw new ArgumentException("mass must be a number");

            Value = value;
        }
    }
}
