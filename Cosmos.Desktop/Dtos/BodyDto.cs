namespace Cosmos.Desktop.Dtos;

public sealed record BodyDto(
    string Name,
    double Mass,
    VectorDto Position,
    VectorDto Velocity);