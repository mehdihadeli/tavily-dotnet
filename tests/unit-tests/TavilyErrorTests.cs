using System.Net;
using Tavily.Net;
using Shouldly;
using Xunit;

public sealed class TavilyErrorTests : TavilyUnitTestBase
{
    [Theory]
    [InlineData(HttpStatusCode.BadRequest, typeof(BadRequestException))]
    [InlineData(HttpStatusCode.Unauthorized, typeof(InvalidApiKeyException))]
    [InlineData(HttpStatusCode.Forbidden, typeof(ForbiddenException))]
    [InlineData(HttpStatusCode.TooManyRequests, typeof(UsageLimitExceededException))]
    public async Task Search_maps_tavily_http_errors_to_typed_exceptions(
        HttpStatusCode statusCode,
        Type exceptionType
    )
    {
        using var client = CreateClient(
            "{\"detail\":{\"error\":\"request failed\"}}",
            _ => Task.CompletedTask,
            statusCode
        );
        var tavily = new TavilyClient(client, "test-key");

        var exception = await Should.ThrowAsync<TavilyException>(
            () => tavily.SearchAsync(new TavilySearchRequest { Query = "dotnet" })
        );

        exception.GetType().ShouldBe(exceptionType);
        exception.Message.ShouldBe("request failed");
    }

    [Fact]
    public async Task Search_maps_keyless_limit_envelope_to_structured_exception()
    {
        using var client = CreateClient(
            "{\"error\":{\"code\":\"rate_limited\",\"message\":\"try later\",\"window\":\"hour\",\"retry_after_seconds\":30,\"next_actions\":[\"wait\"]}}",
            _ => Task.CompletedTask,
            HttpStatusCode.TooManyRequests
        );
        var tavily = new TavilyClient(client, "test-key");

        var exception = await Should.ThrowAsync<TavilyKeylessLimitException>(
            () => tavily.SearchAsync(new TavilySearchRequest { Query = "dotnet" })
        );

        exception.Code.ShouldBe("rate_limited");
        exception.Window.ShouldBe("hour");
        exception.RetryAfterSeconds.ShouldBe(30);
        exception.NextActions.Single().GetString().ShouldBe("wait");
    }
}