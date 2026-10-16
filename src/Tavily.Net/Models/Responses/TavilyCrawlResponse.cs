using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyCrawlResponse
{
    [JsonPropertyName("base_url")]
    public string? BaseUrl { get; init; }

    [JsonPropertyName("results")]
    public IReadOnlyList<TavilyExtractResult> Results { get; init; } = [];

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("usage")]
    public TavilyUsageMetrics? Usage { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

