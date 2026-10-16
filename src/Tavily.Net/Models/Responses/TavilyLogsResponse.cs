using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

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

