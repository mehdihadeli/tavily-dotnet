using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

