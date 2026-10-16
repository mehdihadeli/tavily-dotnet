using System.Text.Json;
using Spectre.Console.Cli;
using Tavily.Net;

var app = new CommandApp<ApiCommand>();
app.Configure(config =>
{
    config.SetApplicationName("tavily-samples");
    config.AddExample("search");
    config.AddExample("research-stream");
});

return app.Run(args);

public sealed class ApiSettings : CommandSettings
{
    [CommandArgument(0, "<operation>")]
    public string Operation { get; set; } = "search";
}

public sealed class ApiCommand : AsyncCommand<ApiSettings>
{
    protected override Task<int> ExecuteAsync(
        CommandContext context,
        ApiSettings settings,
        CancellationToken cancellationToken) =>
        SampleOperations.RunAsync(settings.Operation, cancellationToken);
}

internal static class SampleOperations
{
    public static async Task<int> RunAsync(string operation, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(90) };
        var client = new TavilyClient(httpClient);

        try
        {
            switch (operation.ToLowerInvariant())
            {
                case "search":
                    await SearchAsync(client, cancellationToken);
                    break;
                case "extract":
                    await ExtractAsync(client, cancellationToken);
                    break;
                case "crawl":
                    await CrawlAsync(client, cancellationToken);
                    break;
                case "map":
                    await MapAsync(client, cancellationToken);
                    break;
                case "research":
                    await ResearchAsync(client, cancellationToken);
                    break;
                case "research-stream":
                    await StreamResearchAsync(client, cancellationToken);
                    break;
                case "usage":
                    await UsageAsync(client, cancellationToken);
                    break;
                case "logs":
                    await LogsAsync(client, cancellationToken);
                    break;
                case "organization-usage":
                    await OrganizationUsageAsync(client, cancellationToken);
                    break;
                case "feedback":
                    await FeedbackAsync(client, cancellationToken);
                    break;
                case "company":
                    await CompanyAsync(client, cancellationToken);
                    break;
                case "context":
                    await ContextAsync(client, cancellationToken);
                    break;
                case "qna":
                    await QnaAsync(client, cancellationToken);
                    break;
                default:
                    Console.Error.WriteLine($"Unknown operation '{operation}'.");
                    Console.Error.WriteLine("Try: search, extract, crawl, map, research, research-stream, usage, logs, organization-usage, feedback, company, context, or qna.");
                    return 2;
            }

            return 0;
        }
        catch (TavilyException exception)
        {
            Console.Error.WriteLine($"Tavily request failed: {exception.Message}");
            return 1;
        }
    }

    private static async Task SearchAsync(ITavilyClient client, CancellationToken cancellationToken)
    {
        var response = await client.SearchAsync(new TavilySearchRequest
        {
            Query = "latest .NET release",
            SearchDepth = "advanced",
            MaxResults = 5,
            IncludeAnswer = "advanced",
            IncludeRawContent = "markdown",
        }, cancellationToken);

        Console.WriteLine(response.Answer);
        foreach (var result in response.Results)
        {
            Console.WriteLine($"- {result.Title}: {result.Url}");
        }
    }

    private static async Task ExtractAsync(ITavilyClient client, CancellationToken cancellationToken)
    {
        var response = await client.ExtractAsync(new TavilyExtractRequest
        {
            Urls = ["https://learn.microsoft.com/dotnet/"],
            ExtractDepth = "advanced",
            Format = "markdown",
        }, cancellationToken);

        foreach (var result in response.Results)
        {
            Console.WriteLine(result.RawContent);
        }
    }

    private static async Task CrawlAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.CrawlAsync(new TavilyCrawlRequest
        {
            Url = "https://docs.tavily.com",
            MaxDepth = 2,
            Limit = 20,
            Instructions = "Find documentation pages about search",
        }, cancellationToken));

    private static async Task MapAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.MapAsync(new TavilyMapRequest
        {
            Url = "https://docs.tavily.com",
            MaxDepth = 2,
            Limit = 20,
        }, cancellationToken));

    private static async Task ResearchAsync(ITavilyClient client, CancellationToken cancellationToken)
    {
        var queued = await client.ResearchAsync(new TavilyResearchRequest
        {
            Input = "Research the latest developments in AI",
            Model = "pro",
            CitationFormat = "numbered",
        }, cancellationToken);

        Console.WriteLine($"Request ID: {queued.RequestId}");
        var result = queued;
        while (!string.Equals(result.Status, "completed", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(result.Status, "failed", StringComparison.OrdinalIgnoreCase))
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            result = await client.GetResearchAsync(queued.RequestId!, cancellationToken);
            Console.WriteLine($"Status: {result.Status}");
        }

        PrintJson(result);
    }

    private static async Task StreamResearchAsync(ITavilyClient client, CancellationToken cancellationToken)
    {
        await foreach (var chunk in client.StreamResearchAsync(new TavilyResearchRequest
        {
            Input = "Summarize the current .NET release landscape",
            Model = "pro",
        }, cancellationToken))
        {
            Console.WriteLine(chunk);
        }
    }

    private static async Task UsageAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.GetUsageAsync(
            Environment.GetEnvironmentVariable("TAVILY_PROJECT_ID"), cancellationToken));

    private static async Task LogsAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.GetLogsAsync(new TavilyLogsRequest
        {
            Limit = 25,
            Endpoints = ["search", "extract"],
        }, cancellationToken));

    private static async Task OrganizationUsageAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.GetOrganizationUsageAsync(new TavilyOrgUsageRequest
        {
            OrganizationName = RequiredEnvironmentVariable("TAVILY_ORGANIZATION_NAME"),
            Depth = "project",
        }, cancellationToken));

    private static async Task FeedbackAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        PrintJson(await client.SubmitFeedbackAsync(new TavilyFeedbackRequest
        {
            RequestId = RequiredEnvironmentVariable("TAVILY_REQUEST_ID"),
            AgentScore = 1,
            Comment = "Useful result",
        }, cancellationToken));

    private static async Task CompanyAsync(ITavilyClient client, CancellationToken cancellationToken)
    {
        var results = await client.GetCompanyInfoAsync("Microsoft", maxResults: 3, cancellationToken: cancellationToken);
        foreach (var result in results)
        {
            Console.WriteLine($"- {result.Title}: {result.Url}");
        }
    }

    private static async Task ContextAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        Console.WriteLine(await client.GetSearchContextAsync(new TavilySearchContextRequest
        {
            Query = "What is .NET?",
        }, cancellationToken: cancellationToken));

    private static async Task QnaAsync(ITavilyClient client, CancellationToken cancellationToken) =>
        Console.WriteLine(await client.QnaSearchAsync("What is the latest .NET release?", cancellationToken));

    private static string RequiredEnvironmentVariable(string name) =>
        Environment.GetEnvironmentVariable(name)
        ?? throw new InvalidOperationException($"Set {name} before running this sample.");

    private static void PrintJson<T>(T value) =>
        Console.WriteLine(JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true }));
}
