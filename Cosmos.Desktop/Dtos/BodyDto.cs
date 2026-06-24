using Cosmos.Desktop.Dtos;

public sealed record BodyDto(
    string Name,
    double Mass,
    string Type,
    float Radius,
    string Color,
    VectorDto Position,
    VectorDto Velocity);