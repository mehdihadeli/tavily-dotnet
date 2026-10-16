using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyOrgUsageResponse
{
    [JsonPropertyName("organization")]
    public TavilyOrganizationUsage? Organization { get; init; }

    [JsonPropertyName("totals")]
    public TavilyUsageAggregate? Totals { get; init; }

    [JsonPropertyName("keys")]
    public IReadOnlyList<TavilyUsageKeySummary> Keys { get; init; } = [];
}

