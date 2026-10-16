using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyLogsRequest
{
    [JsonPropertyName("limit")]
    public int Limit { get; init; } = 10;

    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("endpoints")]
    public IReadOnlyList<string>? Endpoints { get; init; }

    [JsonPropertyName("project_id")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("filter_by_api_key")]
    public bool FilterByApiKey { get; init; }
}

