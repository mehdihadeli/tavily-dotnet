using System.Text.Json;

namespace Tavily.Net;

public sealed class TavilyKeylessLimitException : UsageLimitExceededException
{
    public TavilyKeylessLimitException(
        string message,
        string? code = null,
        string? window = null,
        int? retryAfterSeconds = null,
        IReadOnlyList<JsonElement>? nextActions = null
    )
        : base(message)
    {
        Code = code;
        Window = window;
        RetryAfterSeconds = retryAfterSeconds;
        NextActions = nextActions ?? [];
    }

    public string? Code { get; }
    public string? Window { get; }
    public int? RetryAfterSeconds { get; }
    public IReadOnlyList<JsonElement> NextActions { get; }
}

