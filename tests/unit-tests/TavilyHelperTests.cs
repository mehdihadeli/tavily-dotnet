using System.Net.Http.Json;
using System.Text.Json;
using Tavily.Net;
using Shouldly;
using Xunit;

public sealed class TavilyHelperTests : TavilyUnitTestBase
{
    [Fact]
    public async Task Search_context_and_qna_helpers_build_search_requests()
    {
        using var client = CreateClient(
            "{\"answer\":\"42\",\"results\":[{\"url\":\"https://example.com\",\"content\":\"answer\"}]}",
            async request =>
            {
                var body = await request.Content!.ReadFromJsonAsync<JsonElement>();
                body.GetProperty("query").GetString().ShouldBe("meaning");
            }
        );
        var tavily = new TavilyClient(client, "test-key");

        var context = await tavily.GetSearchContextAsync(
            new TavilySearchContextRequest { Query = "meaning" }
        );
        var answer = await tavily.QnaSearchAsync("meaning");

        context.ShouldContain("https://example.com");
        answer.ShouldBe("42");
    }

    [Fact]
    public void Tool_wrappers_expose_search_and_extract_functions()
    {
        var tavily = new TavilyClient(new HttpClient(new StubHandler("{}")), "test-key");

        tavily.AsSearchTool().Name.ShouldBe("tavily_search");
        tavily.AsExtractTool().Name.ShouldBe("tavily_extract");
    }
}
