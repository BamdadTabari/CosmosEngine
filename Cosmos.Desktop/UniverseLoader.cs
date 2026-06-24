using Cosmos.Desktop.Dtos;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using System.Text.Json;

namespace Cosmos.Desktop;

public sealed class UniverseLoader
{
    public Universe Load(
        string filePath)
    {
        var json =
            File.ReadAllText(filePath);

        var bodyDtos =
    JsonSerializer.Deserialize<List<BodyDto>>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })
    ?? [];

        var universe =
            new Universe();

        foreach (var dto in bodyDtos)
        {
            var type =
                Enum.Parse<BodyType>(
                dto.Type);

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

                    dto.Name,
                    type);

            universe.AddBody(body);
        }

        return universe;
    }
}