using System.Text.Json;

namespace Tavily.Net;

public sealed class InvalidApiKeyException : TavilyException
{
    public InvalidApiKeyException(string message)
        : base(message) { }
}

