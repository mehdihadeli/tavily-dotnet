using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

