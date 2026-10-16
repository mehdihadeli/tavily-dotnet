using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyUsageAccount
{
    [JsonPropertyName("current_plan")]
    public string? CurrentPlan { get; init; }

    [JsonPropertyName("plan_usage")]
    public int? PlanUsage { get; init; }

    [JsonPropertyName("plan_limit")]
    public int? PlanLimit { get; init; }

    [JsonPropertyName("paygo_usage")]
    public int? PaygoUsage { get; init; }

    [JsonPropertyName("paygo_limit")]
    public int? PaygoLimit { get; init; }

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

