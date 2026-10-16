using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

