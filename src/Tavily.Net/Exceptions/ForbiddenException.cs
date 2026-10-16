using System.Text.Json;

namespace Tavily.Net;

public sealed class ForbiddenException : TavilyException
{
    public ForbiddenException(string message)
        : base(message) { }
}

