using System.Text.Json;

namespace Tavily.Net;

public class UsageLimitExceededException : TavilyException
{
    public UsageLimitExceededException(string message)
        : base(message) { }
}

