using System.Text.Json;

namespace Tavily.Net;

public sealed class KeylessUnsupportedEndpointException : TavilyException
{
    public KeylessUnsupportedEndpointException(string method)
        : base(
            $"`{method}` is not available in keyless mode. Only `search` and `extract` "
                + "can be called without an API key."
        )
    {
        Method = method;
    }

    public string Method { get; }
}

