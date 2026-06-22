using Cosmos.Domain.Entities;

namespace Cosmos.Desktop;

public sealed class Camera
{
    public Body? Target { get; set; }

    public double AngleX { get; set; }

    public double AngleY { get; set; }

    public double Distance { get; set; } = 300;
}