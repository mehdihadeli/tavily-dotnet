using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyExtractFailure
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }
}

