using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyRequestOptions
{
    public string? ProjectId { get; init; }
    public string? SessionId { get; init; }
    public string? HumanId { get; init; }
    public string? ClientName { get; init; }
}

