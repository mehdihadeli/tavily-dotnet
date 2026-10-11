using System.Net.Http.Json;
using System.Text.Json;
using Tavily.Net;
using Shouldly;
using Xunit;

public sealed class TavilyRequestTests : TavilyUnitTestBase
{
    [Fact]
    public async Task Search_posts_openapi_request_with_bearer_authentication()
    {
        using var client = CreateClient(
            "{\"query\":\"dotnet\",\"answer\":\"answer\",\"results\":[{\"title\":\"Docs\",\"url\":\"https://example.com\",\"content\":\"content\"}]}",
            request =>
            {
                request.Method.ShouldBe(HttpMethod.Post);
                request.RequestUri!.PathAndQuery.ShouldBe("/search");
                request.Headers.Authorization!.Scheme.ShouldBe("Bearer");
                request.Headers.Authorization.Parameter.ShouldBe("test-key");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        var response = await tavily.SearchAsync(
            new TavilySearchRequest { Query = "dotnet", MaxResults = 1 },
            TestContext.Current.CancellationToken
        );

        response.Answer.ShouldBe("answer");
        response.Results.Single().Url.ShouldBe("https://example.com");
    }

    [Fact]
    public async Task Extract_posts_urls_and_returns_extracted_content()
    {
        using var client = CreateClient(
            "{\"results\":[{\"url\":\"https://example.com\",\"raw_content\":\"hello\"}],\"failed_results\":[]}",
            async request =>
            {
                request.RequestUri!.PathAndQuery.ShouldBe("/extract");
                var body = await request.Content!.ReadFromJsonAsync<TavilyExtractRequest>();
                body!.Urls.ShouldContain("https://example.com");
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        var response = await tavily.ExtractAsync(
            new TavilyExtractRequest { Urls = ["https://example.com"] },
            TestContext.Current.CancellationToken
        );

        response.Results.Single().RawContent.ShouldBe("hello");
    }

    [Fact]
    public async Task Search_serializes_extended_openapi_options()
    {
        using var client = CreateClient(
            "{\"query\":\"dotnet\",\"results\":[]}",
            async request =>
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                body.GetProperty("topic").GetString().ShouldBe("news");
                body.GetProperty("include_domains")[0].GetString().ShouldBe("learn.microsoft.com");
                body.GetProperty("include_images").GetBoolean().ShouldBeTrue();
                body.GetProperty("safe_search").GetBoolean().ShouldBeTrue();
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        await tavily.SearchAsync(
            new TavilySearchRequest
            {
                Query = "dotnet",
                Topic = "news",
                IncludeDomains = ["learn.microsoft.com"],
                IncludeImages = true,
                SafeSearch = true,
            }
        );
    }

    [Fact]
    public async Task Crawl_map_and_research_use_documented_endpoints()
    {
        using var client = CreateClient(
            "{\"base_url\":\"https://docs.example.com\",\"results\":[],\"request_id\":\"r1\"}",
            request =>
            {
                request.RequestUri!.PathAndQuery.ShouldBeOneOf("/crawl", "/map", "/research");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        await tavily.CrawlAsync(new TavilyCrawlRequest { Url = "https://docs.example.com" });
        await tavily.MapAsync(new TavilyMapRequest { Url = "https://docs.example.com" });
        await tavily.ResearchAsync(new TavilyResearchRequest { Input = "Summarize .NET 10" });
    }

    [Fact]
    public async Task GetResearch_uses_research_status_endpoint()
    {
        using var client = CreateClient(
            "{\"request_id\":\"r1\",\"status\":\"completed\",\"sources\":[]}",
            request =>
            {
                request.Method.ShouldBe(HttpMethod.Get);
                request.RequestUri!.PathAndQuery.ShouldBe("/research/r1");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        var response = await tavily.GetResearchAsync("r1");

        response.Status.ShouldBe("completed");
    }
}
