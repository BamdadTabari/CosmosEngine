using Cosmos.Desktop.Configs;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Enums;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using System.Text.Json;

namespace Cosmos.Desktop.Loaders;

public sealed class StyleLoader
{
    public BodyStyleConfig Load(
        string path)
    {
        var json =
            File.ReadAllText(path);

        var bodyStyleConfigJson =
            JsonSerializer.Deserialize<BodyStyleConfig>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (bodyStyleConfigJson == null)
            throw new Exception("HOOOOY, fill body-styles.json file idiot");

        return bodyStyleConfigJson;

    }
}