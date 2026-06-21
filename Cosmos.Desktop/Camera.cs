using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;

namespace Cosmos.Desktop;

public sealed class Camera
{
    public Vector2D Position { get; set; }
        = new(0, 0);

    public Body? Target { get; set; }
}