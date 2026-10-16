using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyUsageMetrics
{
    [JsonPropertyName("credits")]
    public double? Credits { get; init; }
}

