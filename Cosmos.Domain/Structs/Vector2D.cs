using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.Structs
{
    public readonly record struct Vector2D(
     double X,
     double Y)
    {
        public static Vector2D operator +(Vector2D a, Vector2D b)
            => new(a.X + b.X, a.Y + b.Y);

        public static Vector2D operator -(Vector2D a, Vector2D b)
            => new(a.X - b.X, a.Y - b.Y);

        public static Vector2D operator *(Vector2D v, double s)
            => new(v.X * s, v.Y * s);

        public static Vector2D operator /(Vector2D v, double s)
            => new(v.X / s, v.Y / s);

        public double Length()
            => Math.Sqrt(X * X + Y * Y);

        public Vector2D Normalize()
        {
            var length = Length();

            if (length == 0)
                return new Vector2D(0, 0);

            return this / length;
        }
    }
}
