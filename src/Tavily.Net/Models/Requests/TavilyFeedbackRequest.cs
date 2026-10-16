using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

