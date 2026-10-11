using Tavily.Net;
using Shouldly;
using Xunit;

namespace AgentSkillsMcp.Search.Tests;

public sealed class TavilyCoreIntegrationTests : TavilyIntegrationTestBase
{
    public static new bool LiveTestsEnabled => TavilyIntegrationTestBase.LiveTestsEnabled;

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Search_returns_live_tavily_results()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.SearchAsync(
            new TavilySearchRequest
            {
                Query = "Microsoft .NET official documentation",
                MaxResults = 1,
                IncludeAnswer = false,
                IncludeRawContent = false,
            },
            cancellationToken
        );

        response.Results.ShouldNotBeEmpty();
        response.Results[0].Url.ShouldNotBeNullOrWhiteSpace();
        response.Results[0].Content.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Extract_returns_live_tavily_content()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.ExtractAsync(
            new TavilyExtractRequest
            {
                Urls =
                [
                    "https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview",
                ],
            },
            cancellationToken
        );

        response.Results.ShouldNotBeEmpty();
        response.Results[0].Url.ShouldNotBeNullOrWhiteSpace();
        response.Results[0].Url!.ShouldContain("learn.microsoft.com");
        response.Results[0].RawContent.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Crawl_returns_live_pages_from_tavily_docs()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.CrawlAsync(
            new TavilyCrawlRequest
            {
                Url = "https://docs.tavily.com",
                MaxDepth = 1,
                MaxBreadth = 3,
                Limit = 3,
                IncludeUsage = true,
            },
            cancellationToken
        );

        response.BaseUrl.ShouldNotBeNullOrWhiteSpace();
        response.Results.ShouldNotBeEmpty();
        response.Results[0].Url.ShouldNotBeNullOrWhiteSpace();
        response.Results[0].RawContent.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Map_returns_live_urls_from_tavily_docs()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.MapAsync(
            new TavilyMapRequest
            {
                Url = "https://docs.tavily.com",
                MaxDepth = 1,
                MaxBreadth = 3,
                Limit = 3,
                IncludeUsage = true,
            },
            cancellationToken
        );

        response.BaseUrl.ShouldNotBeNullOrWhiteSpace();
        response.Results.ShouldNotBeEmpty();
        response.Results[0].ShouldStartWith("http");
    }
}
