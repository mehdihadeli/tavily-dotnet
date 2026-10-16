using System.Text.Json;

namespace Tavily.Net;

public class TavilyException : Exception
{
    public TavilyException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}

