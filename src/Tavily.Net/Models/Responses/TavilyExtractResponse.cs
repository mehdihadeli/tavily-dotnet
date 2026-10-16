using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyExtractResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<TavilyExtractResult> Results { get; init; } = [];

    [JsonPropertyName("failed_results")]
    public IReadOnlyList<TavilyExtractFailure> FailedResults { get; init; } = [];

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("usage")]
    public TavilyUsageMetrics? Usage { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

