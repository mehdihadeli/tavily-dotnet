using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public class TavilyUsageAggregate
{
    [JsonPropertyName("usage")]
    public int? Usage { get; init; }

    [JsonPropertyName("paygo_cost_usd")]
    public double? PaygoCostUsd { get; init; }

    [JsonPropertyName("request_count")]
    public int? RequestCount { get; init; }

    [JsonPropertyName("by_type")]
    public IReadOnlyDictionary<string, TavilyUsageMetrics>? ByType { get; init; }
}

