using Microsoft.Extensions.AI;

namespace Tavily.Net;

public static class TavilyToolExtensions
{
    public static AIFunction AsSearchTool(
        this ITavilyClient client,
        int maxResults = 5,
        string searchDepth = "basic",
        bool includeAnswer = true
    ) =>
        AIFunctionFactory.Create(
            async (string query, CancellationToken cancellationToken = default) =>
                await client.SearchAsync(
                    new TavilySearchRequest
                    {
                        Query = query,
                        MaxResults = maxResults,
                        SearchDepth = searchDepth,
                        IncludeAnswer = includeAnswer,
                    },
                    cancellationToken
                ),
            new AIFunctionFactoryOptions
            {
                Name = "tavily_search",
                Description = "Search the web with Tavily and return ranked sources and content.",
            }
        );

    public static AIFunction AsExtractTool(this ITavilyClient client) =>
        AIFunctionFactory.Create(
            async (string url, CancellationToken cancellationToken = default) =>
                await client.ExtractAsync(
                    new TavilyExtractRequest { Urls = [url] },
                    cancellationToken
                ),
            new AIFunctionFactoryOptions
            {
                Name = "tavily_extract",
                Description = "Extract readable content from a public URL with Tavily.",
            }
        );
}