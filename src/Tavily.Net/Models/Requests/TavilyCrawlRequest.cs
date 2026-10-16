using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

