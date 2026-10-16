using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyExtractResult
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("raw_content")]
    public string? RawContent { get; init; }

    [JsonPropertyName("images")]
    public IReadOnlyList<string> Images { get; init; } = [];

    [JsonPropertyName("favicon")]
    public string? Favicon { get; init; }
}

