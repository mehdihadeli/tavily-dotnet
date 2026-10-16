using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyOrganizationUsage
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("filters")]
    public TavilyUsageFilters? Filters { get; init; }
}

