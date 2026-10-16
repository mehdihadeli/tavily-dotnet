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
    public async Task Map_serializes_include_images()
    {
        using var client = CreateClient(
            "{\"base_url\":\"https://docs.example.com\",\"results\":[]}",
            async request =>
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                body.GetProperty("include_images").GetBoolean().ShouldBeTrue();
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        await tavily.MapAsync(
            new TavilyMapRequest
            {
                Url = "https://docs.example.com",
                IncludeImages = true,
            }
        );
    }

    [Fact]
    public async Task Keyless_search_sends_keyless_headers_without_authorization()
    {
        using var client = CreateClient(
            "{\"query\":\"dotnet\",\"results\":[]}",
            request =>
            {
                request.Headers.Authorization.ShouldBeNull();
                request.Headers.GetValues("X-Tavily-Access-Mode").Single().ShouldBe("keyless");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(
            client,
            envFilePath: Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.env")
        );

        await tavily.SearchAsync(new TavilySearchRequest { Query = "dotnet" });
    }

    [Fact]
    public async Task Keyless_client_rejects_non_search_endpoints()
    {
        using var client = CreateClient("{}", _ => Task.CompletedTask);
        var tavily = new TavilyClient(
            client,
            envFilePath: Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.env")
        );

        await Should.ThrowAsync<KeylessUnsupportedEndpointException>(
            () => tavily.CrawlAsync(new TavilyCrawlRequest { Url = "https://example.com" })
        );
    }

    [Fact]
    public async Task Request_options_override_metadata_headers()
    {
        using var client = CreateClient(
            "{\"query\":\"dotnet\",\"results\":[]}",
            request =>
            {
                request.Headers.GetValues("X-Project-ID").Single().ShouldBe("project-2");
                request.Headers.GetValues("X-Session-Id").Single().ShouldBe("session-2");
                request.Headers.GetValues("X-Human-Id").Single().ShouldBe("human-2");
                request.Headers.GetValues("X-Client-Name").Single().ShouldBe("client-2");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(
            client,
            "test-key",
            projectId: "project-1",
            sessionId: "session-1",
            humanId: "human-1",
            clientName: "client-1"
        );

        await tavily.SearchAsync(
            new TavilySearchRequest { Query = "dotnet" },
            requestOptions: new TavilyRequestOptions
            {
                ProjectId = "project-2",
                SessionId = "session-2",
                HumanId = "human-2",
                ClientName = "client-2",
            }
        );
    }

    [Fact]
    public async Task Stream_research_reads_data_events_until_done()
    {
        using var client = CreateClient(
            "data: {\"content\":\"one\"}\n\ndata: {\"content\":\"two\"}\n\ndata: [DONE]\n",
            async request =>
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                body.GetProperty("stream").GetBoolean().ShouldBeTrue();
                request.RequestUri!.PathAndQuery.ShouldBe("/research");
            }
        );
        var tavily = new TavilyClient(client, "test-key");
        var chunks = new List<string>();

        await foreach (
            var chunk in tavily.StreamResearchAsync(
                new TavilyResearchRequest { Input = "Research .NET" }
            )
        )
        {
            chunks.Add(chunk);
        }

        chunks.ShouldBe(["{\"content\":\"one\"}", "{\"content\":\"two\"}"]);
    }

    [Fact]
    public async Task Research_rejects_stream_flag_when_using_json_response_method()
    {
        using var client = CreateClient("{}", _ => Task.CompletedTask);
        var tavily = new TavilyClient(client, "test-key");

        await Should.ThrowAsync<InvalidOperationException>(
            () => tavily.ResearchAsync(
                new TavilyResearchRequest
                {
                    Input = "Research .NET",
                    Stream = true,
                }
            )
        );
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
