using Cosmos.Desktop.Configs;
using System.Text.Json;

namespace Cosmos.Desktop;

public sealed class StyleLoader
{
    public BodyStyleConfig Load(
        string path)
    {
        var json =
            File.ReadAllText(path);

        return JsonSerializer.Deserialize<BodyStyleConfig>(
            json)!;
    }
}