using Cosmos.Desktop.Configs;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Raylib_cs;

namespace Cosmos.Desktop.Styles;

public sealed class PlanetStyleProvider
{
    private readonly BodyStyleConfig
        _config;

    public PlanetStyleProvider(
        BodyStyleConfig config)
    {
        _config = config;
    }

    public BodyRenderInfo GetStyle(
        Body body)
    {
        var kirkhar = _config.Styles;
        var style =
            _config.Styles[
                body.Type.ToString()];

        return new BodyRenderInfo(
            ParseColor(style.Color),
            CalculateRadius(body)
            * (float)style.RadiusMultiplier);
    }

    private static Color ParseColor(
        string color)
    {
        return color switch
        {
            "Gold" => Color.Gold,
            "SkyBlue" => Color.SkyBlue,
            "LightGray" => Color.LightGray,
            "Brown" => Color.Brown,
            _ => Color.White
        };
    }

    private static float CalculateRadius(
        Body body)
    {
        return (float)Math.Clamp(
            Math.Pow(body.Mass.Value, 0.25),
            2,
            25);
    }
}