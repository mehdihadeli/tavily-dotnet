using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

