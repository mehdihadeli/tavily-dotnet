using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyResearchFile
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; init; } = "base64";
}

