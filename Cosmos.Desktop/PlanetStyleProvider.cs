using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Raylib_cs;

namespace Cosmos.Desktop;

public sealed class PlanetStyleProvider
{
    public BodyRenderInfo GetStyle(
        Body body)
    {
        return body.Type switch
        {
            BodyType.Star =>
                new BodyRenderInfo(
                    Color.Gold,
                    20),

            BodyType.Planet =>
                new BodyRenderInfo(
                    Color.SkyBlue,
                    CalculateRadius(body)),

            BodyType.Moon =>
                new BodyRenderInfo(
                    Color.LightGray,
                    CalculateRadius(body)),

            BodyType.Asteroid =>
                new BodyRenderInfo(
                    Color.Brown,
                    CalculateRadius(body)),

            _ =>
                new BodyRenderInfo(
                    Color.White,
                    CalculateRadius(body))
        };
    }

    private float CalculateRadius(
        Body body)
    {
        return (float)Math.Clamp(
            Math.Pow(body.Mass.Value, 0.25),
            2,
            25);
    }
}