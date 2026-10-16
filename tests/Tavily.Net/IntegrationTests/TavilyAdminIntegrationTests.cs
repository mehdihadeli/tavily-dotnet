using Tavily.Net;
using Shouldly;
using Xunit;

namespace AgentSkillsMcp.Search.Tests;

public sealed class TavilyAdminIntegrationTests : TavilyIntegrationTestBase
{
    public static new bool LiveTestsEnabled => TavilyIntegrationTestBase.LiveTestsEnabled;
    public static new bool OrganizationUsageEnabled =>
        TavilyIntegrationTestBase.OrganizationUsageEnabled;
    public static new bool PaidPlanLiveTestsEnabled =>
        TavilyIntegrationTestBase.PaidPlanLiveTestsEnabled;

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Usage_returns_live_account_details()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.GetUsageAsync(
            Environment.GetEnvironmentVariable("TAVILY_PROJECT_ID"),
            cancellationToken
        );

        response.ShouldNotBeNull();
    }

    [Fact(
        Skip = "Set TAVILY_RUN_PAID_LIVE_TESTS=true and provide a paid-plan API key to run this test.",
        SkipUnless = nameof(PaidPlanLiveTestsEnabled)
    )]
    public async Task Logs_returns_live_request_history()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.GetLogsAsync(
            new TavilyLogsRequest
            {
                Limit = 5,
                ProjectId = Environment.GetEnvironmentVariable("TAVILY_PROJECT_ID"),
            },
            cancellationToken
        );

        response.ShouldNotBeNull();
    }

    [Fact(
        Skip = "Set TAVILY_ORGANIZATION_NAME to run organization usage tests.",
        SkipUnless = nameof(OrganizationUsageEnabled)
    )]
    public async Task Organization_usage_returns_live_aggregates()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var response = await tavily.GetOrganizationUsageAsync(
            new TavilyOrgUsageRequest
            {
                OrganizationName = Environment.GetEnvironmentVariable("TAVILY_ORGANIZATION_NAME")!,
                ProjectId = Environment.GetEnvironmentVariable("TAVILY_PROJECT_ID"),
            },
            cancellationToken
        );

        response.ShouldNotBeNull();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Feedback_accepts_live_search_feedback()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var search = await tavily.SearchAsync(
            new TavilySearchRequest { Query = "Microsoft .NET official documentation" },
            cancellationToken
        );

        search.RequestId.ShouldNotBeNullOrWhiteSpace();
        var response = await tavily.SubmitFeedbackAsync(
            new TavilyFeedbackRequest
            {
                RequestId = search.RequestId,
                AgentScore = 1,
                Comment = "Integration test feedback",
            },
            cancellationToken
        );

        response.ShouldNotBeNull();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Company_info_returns_ranked_live_results()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var results = await tavily.GetCompanyInfoAsync(
            "Microsoft",
            maxResults: 3,
            cancellationToken: cancellationToken
        );

        results.ShouldNotBeEmpty();
        results.Count.ShouldBeLessThanOrEqualTo(3);
    }
}
