using Tavily.Net;

namespace AgentSkillsMcp.Search.Tests;

public abstract class TavilyIntegrationTestBase
{
    static TavilyIntegrationTestBase()
    {
        TavilyApiKeyResolver.LoadDotEnv();
    }

    public static bool LiveTestsEnabled =>
        !string.IsNullOrWhiteSpace(TavilyApiKeyResolver.Resolve());

    public static bool OrganizationUsageEnabled =>
        LiveTestsEnabled
        && !string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable("TAVILY_ORGANIZATION_NAME")
        );

    protected static (TavilyClient Client, CancellationToken CancellationToken) CreateLiveClient(
        TimeSpan? timeout = null
    )
    {
        var apiKey = TavilyApiKeyResolver.Resolve();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "TAVILY_API_KEY was not found in the environment or .env."
            );
        }

        var httpClient = new HttpClient { Timeout = timeout ?? TimeSpan.FromSeconds(90) };
        return (new TavilyClient(httpClient, apiKey), TestContext.Current.CancellationToken);
    }
}