using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Tavily.Net;

public sealed class TavilyToolExtensionsTests : TavilyUnitTestBase
{
    [Fact]
    public async Task Search_tool_exposes_metadata_and_delegates_to_client()
    {
        using var client = CreateClient(
            "{\"query\":\"dotnet\",\"results\":[]}",
            request =>
            {
                request.RequestUri!.PathAndQuery.ShouldBe("/search");
                return Task.CompletedTask;
            }
        );
        var tavily = new TavilyClient(client, "test-key");
        var tool = tavily.AsSearchTool();

        tool.Name.ShouldBe("tavily_search");
        tool.Description.ShouldBe("Search the web with Tavily and return ranked sources and content.");
        var result = await tool.InvokeAsync(new AIFunctionArguments { ["query"] = "dotnet" });

        var json = result.ShouldBeOfType<JsonElement>();
        json.GetProperty("query").GetString().ShouldBe("dotnet");
    }

    [Fact]
    public async Task Extract_tool_delegates_url_to_client()
    {
        using var client = CreateClient(
            "{\"results\":[{\"url\":\"https://example.com\",\"raw_content\":\"content\"}],\"failed_results\":[]}",
            async request =>
            {
                request.RequestUri!.PathAndQuery.ShouldBe("/extract");
                var body = await request.Content!.ReadFromJsonAsync<TavilyExtractRequest>();
                body!.Urls.ShouldContain("https://example.com");
            }
        );
        var tavily = new TavilyClient(client, "test-key");
        var tool = tavily.AsExtractTool();

        tool.Name.ShouldBe("tavily_extract");
        var result = await tool.InvokeAsync(
            new AIFunctionArguments { ["url"] = "https://example.com" }
        );

        var json = result.ShouldBeOfType<JsonElement>();
        json.GetProperty("results")[0].GetProperty("raw_content").GetString().ShouldBe("content");
    }
}