using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyUsageResponse
{
    [JsonPropertyName("key")]
    public TavilyUsageKey? Key { get; init; }

    [JsonPropertyName("account")]
    public TavilyUsageAccount? Account { get; init; }
}

