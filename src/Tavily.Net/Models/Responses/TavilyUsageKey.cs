using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyUsageKey
{
    [JsonPropertyName("usage")]
    public int? Usage { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("search_usage")]
    public int? SearchUsage { get; init; }

    [JsonPropertyName("extract_usage")]
    public int? ExtractUsage { get; init; }

    [JsonPropertyName("crawl_usage")]
    public int? CrawlUsage { get; init; }

    [JsonPropertyName("map_usage")]
    public int? MapUsage { get; init; }

    [JsonPropertyName("research_usage")]
    public int? ResearchUsage { get; init; }
}

