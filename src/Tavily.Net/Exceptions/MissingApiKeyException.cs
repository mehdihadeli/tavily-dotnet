using System.Text.Json;

namespace Tavily.Net;

public sealed class MissingApiKeyException : ArgumentException
{
    public MissingApiKeyException()
        : base("No API key provided. Please provide the apiKey argument.", "apiKey") { }
}

