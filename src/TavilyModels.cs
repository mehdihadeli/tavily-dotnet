using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilySearchRequest
{
    [JsonPropertyName("query")]
    public required string Query { get; init; }

    [JsonPropertyName("search_depth")]
    public string SearchDepth { get; init; } = "advanced";

    [JsonPropertyName("max_results")]
    public int MaxResults { get; init; } = 5;

    [JsonPropertyName("include_answer")]
    public object IncludeAnswer { get; init; } = true;

    [JsonPropertyName("include_raw_content")]
    public object IncludeRawContent { get; init; } = true;

    [JsonPropertyName("topic")]
    public string? Topic { get; init; }

    [JsonPropertyName("time_range")]
    public string? TimeRange { get; init; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("include_published_date")]
    public bool IncludePublishedDate { get; init; }

    [JsonPropertyName("filter_by_published_date")]
    public bool FilterByPublishedDate { get; init; }

    [JsonPropertyName("days")]
    public int? Days { get; init; }

    [JsonPropertyName("include_domains")]
    public IReadOnlyList<string>? IncludeDomains { get; init; }

    [JsonPropertyName("exclude_domains")]
    public IReadOnlyList<string>? ExcludeDomains { get; init; }

    [JsonPropertyName("include_domains_mode")]
    public string? IncludeDomainsMode { get; init; }

    [JsonPropertyName("include_images")]
    public bool IncludeImages { get; init; }

    [JsonPropertyName("include_image_descriptions")]
    public bool IncludeImageDescriptions { get; init; }

    [JsonPropertyName("include_favicon")]
    public bool IncludeFavicon { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("language")]
    public string? Language { get; init; }

    [JsonPropertyName("filter_by_language")]
    public bool FilterByLanguage { get; init; }

    [JsonPropertyName("auto_parameters")]
    public bool AutoParameters { get; init; }

    [JsonPropertyName("exact_match")]
    public bool ExactMatch { get; init; }

    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; init; }

    [JsonPropertyName("safe_search")]
    public bool SafeSearch { get; init; }

    [JsonPropertyName("chunks_per_source")]
    public int? ChunksPerSource { get; init; }
}

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

public sealed class TavilyImage
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

public sealed class TavilyExtractRequest
{
    [JsonPropertyName("urls")]
    public required IReadOnlyList<string> Urls { get; init; }

    [JsonPropertyName("query")]
    public string? Query { get; init; }

    [JsonPropertyName("extract_depth")]
    public string ExtractDepth { get; init; } = "basic";

    [JsonPropertyName("format")]
    public string Format { get; init; } = "markdown";

    [JsonPropertyName("include_images")]
    public bool IncludeImages { get; init; }

    [JsonPropertyName("include_favicon")]
    public bool IncludeFavicon { get; init; }

    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; init; }

    [JsonPropertyName("timeout")]
    public double? Timeout { get; init; }

    [JsonPropertyName("chunks_per_source")]
    public int? ChunksPerSource { get; init; }
}

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

public sealed class TavilyExtractFailure
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }
}

public sealed class TavilyCrawlRequest
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("instructions")]
    public string? Instructions { get; init; }

    [JsonPropertyName("max_depth")]
    public int? MaxDepth { get; init; }

    [JsonPropertyName("max_breadth")]
    public int? MaxBreadth { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("select_paths")]
    public IReadOnlyList<string>? SelectPaths { get; init; }

    [JsonPropertyName("select_domains")]
    public IReadOnlyList<string>? SelectDomains { get; init; }

    [JsonPropertyName("exclude_paths")]
    public IReadOnlyList<string>? ExcludePaths { get; init; }

    [JsonPropertyName("exclude_domains")]
    public IReadOnlyList<string>? ExcludeDomains { get; init; }

    [JsonPropertyName("allow_external")]
    public bool? AllowExternal { get; init; }

    [JsonPropertyName("timeout")]
    public double? Timeout { get; init; }

    [JsonPropertyName("include_images")]
    public bool IncludeImages { get; init; }

    [JsonPropertyName("extract_depth")]
    public string? ExtractDepth { get; init; }

    [JsonPropertyName("format")]
    public string? Format { get; init; }

    [JsonPropertyName("include_favicon")]
    public bool IncludeFavicon { get; init; }

    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; init; }

    [JsonPropertyName("chunks_per_source")]
    public int? ChunksPerSource { get; init; }
}

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

public sealed class TavilyMapRequest
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("instructions")]
    public string? Instructions { get; init; }

    [JsonPropertyName("max_depth")]
    public int? MaxDepth { get; init; }

    [JsonPropertyName("max_breadth")]
    public int? MaxBreadth { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("select_paths")]
    public IReadOnlyList<string>? SelectPaths { get; init; }

    [JsonPropertyName("select_domains")]
    public IReadOnlyList<string>? SelectDomains { get; init; }

    [JsonPropertyName("exclude_paths")]
    public IReadOnlyList<string>? ExcludePaths { get; init; }

    [JsonPropertyName("exclude_domains")]
    public IReadOnlyList<string>? ExcludeDomains { get; init; }

    [JsonPropertyName("allow_external")]
    public bool? AllowExternal { get; init; }

    [JsonPropertyName("timeout")]
    public double? Timeout { get; init; }

    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; init; }
}

public sealed class TavilyMapResponse
{
    [JsonPropertyName("base_url")]
    public string? BaseUrl { get; init; }

    [JsonPropertyName("results")]
    public IReadOnlyList<string> Results { get; init; } = [];

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("usage")]
    public TavilyUsageMetrics? Usage { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

public sealed class TavilyResearchRequest
{
    [JsonPropertyName("input")]
    public required string Input { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("output_schema")]
    public object? OutputSchema { get; init; }

    [JsonPropertyName("stream")]
    public bool Stream { get; init; }

    [JsonPropertyName("citation_format")]
    public string CitationFormat { get; init; } = "numbered";

    [JsonPropertyName("include_domains")]
    public IReadOnlyList<string>? IncludeDomains { get; init; }

    [JsonPropertyName("exclude_domains")]
    public IReadOnlyList<string>? ExcludeDomains { get; init; }

    [JsonPropertyName("output_length")]
    public string? OutputLength { get; init; }

    [JsonPropertyName("files")]
    public IReadOnlyList<TavilyResearchFile>? Files { get; init; }
}

public sealed class TavilyResearchFile
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; init; } = "base64";
}

public sealed class TavilyResearchResponse
{
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; init; }

    [JsonPropertyName("completed_at")]
    public string? CompletedAt { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("input")]
    public string? Input { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("content")]
    public JsonElement? Content { get; init; }

    [JsonPropertyName("sources")]
    public IReadOnlyList<TavilyResearchSource> Sources { get; init; } = [];

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }
}

public sealed class TavilyResearchSource
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("favicon")]
    public string? Favicon { get; init; }
}

public sealed class TavilyUsageMetrics
{
    [JsonPropertyName("credits")]
    public double? Credits { get; init; }
}

public sealed class TavilySearchContextRequest
{
    [JsonPropertyName("query")]
    public required string Query { get; init; }

    [JsonPropertyName("search_depth")]
    public string SearchDepth { get; init; } = "basic";

    [JsonPropertyName("topic")]
    public string Topic { get; init; } = "general";

    [JsonPropertyName("days")]
    public int Days { get; init; } = 7;

    [JsonPropertyName("max_results")]
    public int MaxResults { get; init; } = 5;

    [JsonPropertyName("include_domains")]
    public IReadOnlyList<string>? IncludeDomains { get; init; }

    [JsonPropertyName("exclude_domains")]
    public IReadOnlyList<string>? ExcludeDomains { get; init; }
}

public sealed class TavilyUsageResponse
{
    [JsonPropertyName("key")]
    public TavilyUsageKey? Key { get; init; }

    [JsonPropertyName("account")]
    public TavilyUsageAccount? Account { get; init; }
}

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

public sealed class TavilyLogsRequest
{
    [JsonPropertyName("limit")]
    public int Limit { get; init; } = 10;

    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("endpoints")]
    public IReadOnlyList<string>? Endpoints { get; init; }

    [JsonPropertyName("project_id")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("filter_by_api_key")]
    public bool FilterByApiKey { get; init; }
}

public sealed class TavilyLogsResponse
{
    [JsonPropertyName("logs")]
    public IReadOnlyList<TavilyUsageLog> Logs { get; init; } = [];

    [JsonPropertyName("count")]
    public int? Count { get; init; }

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

public sealed class TavilyUsageLog
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; init; }

    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; init; }

    [JsonPropertyName("depth")]
    public string? Depth { get; init; }

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("credits")]
    public double? Credits { get; init; }

    [JsonPropertyName("api_key")]
    public string? ApiKey { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }
}

public sealed class TavilyOrgUsageRequest
{
    [JsonPropertyName("organization_name")]
    public required string OrganizationName { get; init; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("project_id")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("depth")]
    public string? Depth { get; init; }
}

public sealed class TavilyOrgUsageResponse
{
    [JsonPropertyName("organization")]
    public TavilyOrganizationUsage? Organization { get; init; }

    [JsonPropertyName("totals")]
    public TavilyUsageAggregate? Totals { get; init; }

    [JsonPropertyName("keys")]
    public IReadOnlyList<TavilyUsageKeySummary> Keys { get; init; } = [];
}

public sealed class TavilyOrganizationUsage
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("filters")]
    public TavilyUsageFilters? Filters { get; init; }
}

public sealed class TavilyUsageFilters
{
    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("project_id")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("depth")]
    public string? Depth { get; init; }
}

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

public sealed class TavilyUsageKeySummary : TavilyUsageAggregate
{
    [JsonPropertyName("key")]
    public string? Key { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

public sealed class TavilyFeedbackRequest
{
    [JsonPropertyName("session_id")]
    public string? SessionId { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    [JsonPropertyName("agent_score")]
    public object? AgentScore { get; init; }

    [JsonPropertyName("human_score")]
    public object? HumanScore { get; init; }

    [JsonPropertyName("extra_scores")]
    public IReadOnlyList<object>? ExtraScores { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("response_delivered")]
    public string? ResponseDelivered { get; init; }

    [JsonPropertyName("used_urls")]
    public IReadOnlyList<string>? UsedUrls { get; init; }

    [JsonPropertyName("used_ids")]
    public IReadOnlyList<string>? UsedIds { get; init; }

    [JsonPropertyName("used_citations")]
    public IReadOnlyList<string>? UsedCitations { get; init; }

    [JsonPropertyName("urls_scores")]
    public IReadOnlyList<object>? UrlsScores { get; init; }
}

public sealed class TavilyFeedbackResponse
{
    [JsonPropertyName("success")]
    public bool? Success { get; init; }

    [JsonPropertyName("feedback_id")]
    public string? FeedbackId { get; init; }

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }
}
