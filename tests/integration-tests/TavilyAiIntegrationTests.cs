using Tavily.Net;
using Shouldly;
using Xunit;

namespace AgentSkillsMcp.Search.Tests;

public sealed class TavilyAiIntegrationTests : TavilyIntegrationTestBase
{
    public static new bool LiveTestsEnabled => TavilyIntegrationTestBase.LiveTestsEnabled;

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Search_context_returns_serialized_live_sources()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var context = await tavily.GetSearchContextAsync(
            new TavilySearchContextRequest
            {
                Query = "Microsoft .NET official documentation",
                MaxResults = 2,
            },
            cancellationToken: cancellationToken
        );

        context.ShouldContain("learn.microsoft.com");
        context.Length.ShouldBeGreaterThan(20);
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Qna_search_returns_live_answer()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var answer = await tavily.QnaSearchAsync("What is Microsoft .NET?", cancellationToken);

        answer.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Research_can_be_created_and_polled_to_completion()
    {
        var (tavily, cancellationToken) = CreateLiveClient(TimeSpan.FromMinutes(3));
        var queued = await tavily.ResearchAsync(
            new TavilyResearchRequest
            {
                Input = "Give a brief summary of Microsoft .NET from official sources.",
                Model = "mini",
                OutputLength = "short",
                IncludeDomains = ["learn.microsoft.com"],
            },
            cancellationToken
        );

        queued.RequestId.ShouldNotBeNullOrWhiteSpace();
        TavilyResearchResponse result = queued;
        for (var attempt = 0; attempt < 24; attempt++)
        {
            if (result.Status is "completed" or "failed")
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            result = await tavily.GetResearchAsync(
                result.RequestId!,
                cancellationToken,
                includeUsage: true
            );
        }

        result.Status.ShouldBe("completed");
        result.Content.ShouldNotBeNull();
        result.Sources.ShouldNotBeEmpty();
    }
}
