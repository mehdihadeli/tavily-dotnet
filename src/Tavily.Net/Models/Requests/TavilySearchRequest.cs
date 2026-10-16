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

