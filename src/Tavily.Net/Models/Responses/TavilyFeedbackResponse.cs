using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyFeedbackResponse
{
    [JsonPropertyName("success")]
    public bool? Success { get; init; }

    [JsonPropertyName("feedback_id")]
    public string? FeedbackId { get; init; }

    [JsonPropertyName("response_time")]
    public double? ResponseTime { get; init; }
}

