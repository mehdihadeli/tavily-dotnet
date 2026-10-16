using System.Text.Json;

namespace Tavily.Net;

public sealed class TavilyTimeoutException : TavilyException
{
    public TavilyTimeoutException(TimeSpan timeout)
        : base($"Request timed out after {timeout.TotalSeconds:0.###} seconds.") { }
}

