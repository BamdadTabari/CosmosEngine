using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;

namespace Cosmos.Domain.Entities;

public sealed class Spacecraft : Body
{
    public Spacecraft(
        Vector3D position,
        Vector3D velocity,
        Mass mass,
        string name)
        : base(
            position,
            velocity,
            mass,
            name,
            BodyType.Spacecraft)
    {
    }
}