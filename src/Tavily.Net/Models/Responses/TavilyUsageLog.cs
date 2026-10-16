using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

