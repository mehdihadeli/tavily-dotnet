namespace Tavily.Net;

public interface ITavilyClient
{
    Task<TavilySearchResponse> SearchAsync(
        TavilySearchRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyExtractResponse> ExtractAsync(
        TavilyExtractRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyCrawlResponse> CrawlAsync(
        TavilyCrawlRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyMapResponse> MapAsync(
        TavilyMapRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyResearchResponse> ResearchAsync(
        TavilyResearchRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyResearchResponse> GetResearchAsync(
        string requestId,
        CancellationToken cancellationToken = default,
        bool includeUsage = false,
        TavilyRequestOptions? requestOptions = null
    );

    IAsyncEnumerable<string> StreamResearchAsync(
        TavilyResearchRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyUsageResponse> GetUsageAsync(
        string? projectId = null,
        CancellationToken cancellationToken = default
    );

    Task<TavilyLogsResponse> GetLogsAsync(
        TavilyLogsRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyOrgUsageResponse> GetOrganizationUsageAsync(
        TavilyOrgUsageRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<TavilyFeedbackResponse> SubmitFeedbackAsync(
        TavilyFeedbackRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    );

    Task<IReadOnlyList<TavilySearchResult>> GetCompanyInfoAsync(
        string query,
        string searchDepth = "advanced",
        int maxResults = 5,
        string? country = null,
        CancellationToken cancellationToken = default
    );

    Task<string> GetSearchContextAsync(
        TavilySearchContextRequest request,
        int maxCharacters = 16_000,
        CancellationToken cancellationToken = default
    );

    Task<string?> QnaSearchAsync(string query, CancellationToken cancellationToken = default);
}
