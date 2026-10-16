using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilySearchResponse
{
    [JsonPropertyName("query")]
    public string? Query { get; init; }

    [JsonPropertyName("answer")]
    public string? Answer { get; init; }

    [JsonPropertyName("results")]
    public IReadOnlyList<TavilySearchResult> Results { get; init; } = [];

    [JsonPropertyName("images")]
    public IReadOnlyList<TavilyImage> Images { get; init; } = [];

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("usage")]
    public TavilyUsageMetrics? Usage { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

