using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyUsageKeySummary : TavilyUsageAggregate
{
    [JsonPropertyName("key")]
    public string? Key { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

