using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilySearchResult
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("raw_content")]
    public string? RawContent { get; init; }

    [JsonPropertyName("score")]
    public double? Score { get; init; }

    [JsonPropertyName("favicon")]
    public string? Favicon { get; init; }

    [JsonPropertyName("images")]
    public IReadOnlyList<TavilyImage> Images { get; init; } = [];

    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

