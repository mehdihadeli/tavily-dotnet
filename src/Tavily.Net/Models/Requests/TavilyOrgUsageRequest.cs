using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyOrgUsageRequest
{
    [JsonPropertyName("organization_name")]
    public required string OrganizationName { get; init; }

    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    [JsonPropertyName("project_id")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("depth")]
    public string? Depth { get; init; }
}

