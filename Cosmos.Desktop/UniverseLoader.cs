using System.Text.Json;
using Cosmos.Desktop.Dtos;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;

namespace Cosmos.Desktop;

public sealed class UniverseLoader
{
    public Universe Load(
        string filePath)
    {
        var json =
            File.ReadAllText(filePath);

        var bodyDtos =
            JsonSerializer.Deserialize<List<BodyDto>>(json)
            ?? [];

        var universe =
            new Universe();

        foreach (var dto in bodyDtos)
        {
            var body =
                new Body(
                    new Vector3D(
                        dto.Position.X,
                        dto.Position.Y,
                        dto.Position.Z),

                    new Vector3D(
                        dto.Velocity.X,
                        dto.Velocity.Y,
                        dto.Velocity.Z),

                    new Mass(dto.Mass),

                    dto.Name);

            universe.AddBody(body);
        }

        return universe;
    }
}