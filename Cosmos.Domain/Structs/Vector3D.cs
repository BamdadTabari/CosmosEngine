namespace Cosmos.Domain.Structs;

public sealed record Vector3D(
    double X,
    double Y,
    double Z)
{
    public double LengthSquared()
    {
        return
            X * X +
            Y * Y +
            Z * Z;
    }

    public Vector3D Normalize()
    {
        var length =
            Math.Sqrt(
                LengthSquared());

        if (length == 0)
        {
            return new Vector3D(
                0,
                0,
                0);
        }

        return new Vector3D(
            X / length,
            Y / length,
            Z / length);
    }

    public static Vector3D operator +(
        Vector3D a,
        Vector3D b)
    {
        return new Vector3D(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);
    }

    public static Vector3D operator *(
        Vector3D a,
        double scalar)
    {
        return new Vector3D(
            a.X * scalar,
            a.Y * scalar,
            a.Z * scalar);
    }
}