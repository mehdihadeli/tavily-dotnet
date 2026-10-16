using System.Text.Json;

namespace Tavily.Net;

public sealed class BadRequestException : TavilyException
{
    public BadRequestException(string message)
        : base(message) { }
}

