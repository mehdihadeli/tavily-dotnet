using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavily.Net;

public sealed class TavilyClient : ITavilyClient
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
    private readonly bool _keyless;
    private readonly string _clientSource;
    private readonly Dictionary<string, string> _defaultHeaders;

    public TavilyClient(
        HttpClient httpClient,
        string? apiKey = null,
        string? envFilePath = null,
        string? clientSource = null,
        string? projectId = null,
        string? orgId = null,
        string? sessionId = null,
        string? humanId = null,
        string? clientName = null
    )
    {
        if (httpClient is null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }
        if (apiKey is not null && string.IsNullOrWhiteSpace(apiKey))
        {
            throw new MissingApiKeyException();
        }

        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri("https://api.tavily.com/", UriKind.Absolute);
        _apiKey = TavilyApiKeyResolver.Resolve(apiKey, envFilePath);
        _keyless = _apiKey is null;
        _clientSource = clientSource ?? (_keyless ? "tavily-dotnet-keyless" : "tavily-dotnet");
        _defaultHeaders = new Dictionary<string, string>
        {
            ["X-Client-Source"] = _clientSource,
        };
        if (_keyless)
        {
            _defaultHeaders["X-Tavily-Access-Mode"] = "keyless";
        }
        AddHeader(_defaultHeaders, "X-Project-ID", projectId);
        AddHeader(_defaultHeaders, "X-Tavily-Orgid", orgId);
        AddHeader(_defaultHeaders, "X-Session-Id", sessionId);
        AddHeader(_defaultHeaders, "X-Human-Id", humanId);
        AddHeader(_defaultHeaders, "X-Client-Name", clientName);
    }

    public Task<TavilySearchResponse> SearchAsync(
        TavilySearchRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    ) => PostAsync<TavilySearchRequest, TavilySearchResponse>("search", request, cancellationToken, requestOptions);

    public Task<TavilyExtractResponse> ExtractAsync(
        TavilyExtractRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    ) =>
        PostAsync<TavilyExtractRequest, TavilyExtractResponse>(
            "extract",
            request,
            cancellationToken,
            requestOptions
        );

    public Task<TavilyCrawlResponse> CrawlAsync(
        TavilyCrawlRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(CrawlAsync));
        return PostAsync<TavilyCrawlRequest, TavilyCrawlResponse>("crawl", request, cancellationToken, requestOptions);
    }

    public Task<TavilyMapResponse> MapAsync(
        TavilyMapRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(MapAsync));
        return PostAsync<TavilyMapRequest, TavilyMapResponse>("map", request, cancellationToken, requestOptions);
    }

    public Task<TavilyResearchResponse> ResearchAsync(
        TavilyResearchRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(ResearchAsync));
        if (request.Stream)
        {
            throw new InvalidOperationException(
                "Use StreamResearchAsync when TavilyResearchRequest.Stream is true."
            );
        }

        return PostAsync<TavilyResearchRequest, TavilyResearchResponse>(
            "research",
            request,
            cancellationToken,
            requestOptions
        );
    }

    public Task<TavilyResearchResponse> GetResearchAsync(
        string requestId,
        CancellationToken cancellationToken = default,
        bool includeUsage = false,
        TavilyRequestOptions? requestOptions = null
    )
    {
        if (string.IsNullOrWhiteSpace(requestId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(requestId));
        }
        var path = $"research/{Uri.EscapeDataString(requestId)}";
        if (includeUsage)
        {
            path += "?include_usage=true";
        }
        EnsureKeyed(nameof(GetResearchAsync));
        return GetAsync<TavilyResearchResponse>(path, cancellationToken, requestOptions);
    }

    public async IAsyncEnumerable<string> StreamResearchAsync(
        TavilyResearchRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(StreamResearchAsync));
        var streamingRequest = new TavilyResearchRequest
        {
            Input = request.Input,
            Model = request.Model,
            OutputSchema = request.OutputSchema,
            Stream = true,
            CitationFormat = request.CitationFormat,
            IncludeDomains = request.IncludeDomains,
            ExcludeDomains = request.ExcludeDomains,
            OutputLength = request.OutputLength,
            Files = request.Files,
        };

        using var message = CreateRequest(HttpMethod.Post, "research", requestOptions);
        message.Content = JsonContent.Create(streamingRequest, options: JsonOptions);
        using var response = await SendAsync(message, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            await ThrowForErrorAsync(response, "research", cancellationToken).ConfigureAwait(false);
        }

        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
        string? line;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) is not null)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var data = line.Substring("data:".Length).Trim();
            if (!string.Equals(data, "[DONE]", StringComparison.Ordinal))
            {
                yield return data;
            }
        }
    }

    public Task<TavilyUsageResponse> GetUsageAsync(
        string? projectId = null,
        CancellationToken cancellationToken = default
    )
    {
        EnsureKeyed(nameof(GetUsageAsync));
        return GetUsageCoreAsync(projectId, cancellationToken);
    }

    private Task<TavilyUsageResponse> GetUsageCoreAsync(
        string? projectId,
        CancellationToken cancellationToken
    ) =>
        GetAsync<TavilyUsageResponse>(
            "usage",
            cancellationToken,
            projectId is null ? null : new TavilyRequestOptions { ProjectId = projectId }
        );

    public Task<TavilyLogsResponse> GetLogsAsync(
        TavilyLogsRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(GetLogsAsync));
        return PostAsync<TavilyLogsRequest, TavilyLogsResponse>("logs", request, cancellationToken, requestOptions);
    }

    public Task<TavilyOrgUsageResponse> GetOrganizationUsageAsync(
        TavilyOrgUsageRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(GetOrganizationUsageAsync));
        return PostAsync<TavilyOrgUsageRequest, TavilyOrgUsageResponse>(
            "org-usage",
            request,
            cancellationToken,
            requestOptions
        );
    }

    public Task<TavilyFeedbackResponse> SubmitFeedbackAsync(
        TavilyFeedbackRequest request,
        CancellationToken cancellationToken = default,
        TavilyRequestOptions? requestOptions = null
    )
    {
        EnsureKeyed(nameof(SubmitFeedbackAsync));
        return PostAsync<TavilyFeedbackRequest, TavilyFeedbackResponse>(
            "feedback",
            request,
            cancellationToken,
            requestOptions
        );
    }

    public async Task<IReadOnlyList<TavilySearchResult>> GetCompanyInfoAsync(
        string query,
        string searchDepth = "advanced",
        int maxResults = 5,
        string? country = null,
        CancellationToken cancellationToken = default
    )
    {
        var topics = new[] { "news", "general", "finance" };
        var responses = await Task.WhenAll(
                topics.Select(topic =>
                    SearchAsync(
                        new TavilySearchRequest
                        {
                            Query = query,
                            SearchDepth = searchDepth,
                            Topic = topic,
                            MaxResults = maxResults,
                            IncludeAnswer = false,
                            Country = country,
                        },
                        cancellationToken
                    )
                )
            )
            .ConfigureAwait(false);

        return responses
            .SelectMany(response => response.Results)
            .OrderByDescending(result => result.Score ?? double.MinValue)
            .Take(maxResults)
            .ToArray();
    }

    public async Task<string> GetSearchContextAsync(
        TavilySearchContextRequest request,
        int maxCharacters = 16_000,
        CancellationToken cancellationToken = default
    )
    {
        if (maxCharacters <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCharacters));
        }
        var response = await SearchAsync(
                new TavilySearchRequest
                {
                    Query = request.Query,
                    SearchDepth = request.SearchDepth,
                    MaxResults = request.MaxResults,
                    IncludeAnswer = false,
                    IncludeRawContent = false,
                    Topic = request.Topic,
                    Days = request.Days,
                    IncludeDomains = request.IncludeDomains,
                    ExcludeDomains = request.ExcludeDomains,
                },
                cancellationToken
            )
            .ConfigureAwait(false);

        var context = JsonSerializer.Serialize(
            response.Results.Select(result => new { result.Url, result.Content }),
            JsonOptions
        );
        return context.Length <= maxCharacters ? context : context.Substring(0, maxCharacters);
    }

    public async Task<string?> QnaSearchAsync(
        string query,
        CancellationToken cancellationToken = default
    )
    {
        var response = await SearchAsync(
                new TavilySearchRequest
                {
                    Query = query,
                    SearchDepth = "advanced",
                    IncludeAnswer = true,
                    IncludeRawContent = false,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        return response.Answer;
    }

    private async Task<TResponse> PostAsync<TRequest, TResponse>(
        string path,
        TRequest request,
        CancellationToken cancellationToken,
        TavilyRequestOptions? requestOptions = null
    )
    {
        using var message = CreateRequest(HttpMethod.Post, path, requestOptions);
        message.Content = JsonContent.Create(request, options: JsonOptions);

        var response = await SendAsync(message, cancellationToken).ConfigureAwait(false);

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                await ThrowForErrorAsync(response, path, cancellationToken).ConfigureAwait(false);
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(
                    JsonOptions,
                    cancellationToken
                ) ?? throw new HttpRequestException($"Tavily returned an empty {path} response.");
        }
    }

    private async Task<TResponse> GetAsync<TResponse>(
        string path,
        CancellationToken cancellationToken,
        TavilyRequestOptions? requestOptions = null
    )
    {
        using var message = CreateRequest(HttpMethod.Get, path, requestOptions);

        var response = await SendAsync(message, cancellationToken).ConfigureAwait(false);

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                await ThrowForErrorAsync(response, path, cancellationToken).ConfigureAwait(false);
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(
                    JsonOptions,
                    cancellationToken
                ) ?? throw new HttpRequestException($"Tavily returned an empty {path} response.");
        }
    }

    private HttpRequestMessage CreateRequest(
        HttpMethod method,
        string path,
        TavilyRequestOptions? requestOptions
    )
    {
        var message = new HttpRequestMessage(method, path);
        if (_apiKey is not null)
        {
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        foreach (var header in _defaultHeaders)
        {
            message.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (requestOptions is not null)
        {
            AddHeader(message, "X-Project-ID", requestOptions.ProjectId);
            AddHeader(message, "X-Session-Id", requestOptions.SessionId);
            AddHeader(message, "X-Human-Id", requestOptions.HumanId);
            AddHeader(message, "X-Client-Name", requestOptions.ClientName);
        }

        return message;
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage message,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await _httpClient
                .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TavilyTimeoutException(_httpClient.Timeout);
        }
    }

    private void EnsureKeyed(string method)
    {
        if (_keyless)
        {
            throw new KeylessUnsupportedEndpointException(method);
        }
    }

    private static void AddHeader(
        IDictionary<string, string> headers,
        string name,
        string? value
    )
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            headers[name] = value!;
        }
    }

    private static void AddHeader(
        HttpRequestMessage message,
        string name,
        string? value
    )
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            message.Headers.Remove(name);
            message.Headers.TryAddWithoutValidation(name, value);
        }
    }

    private static async Task ThrowForErrorAsync(
        HttpResponseMessage response,
        string path,
        CancellationToken cancellationToken
    )
    {
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        using var document = TryParse(body);

        if (
            document?.RootElement.TryGetProperty("error", out var error) == true
            && error.ValueKind == JsonValueKind.Object
            && error.TryGetProperty("code", out var code)
            && code.ValueKind == JsonValueKind.String
        )
        {
            var nextActions =
                error.TryGetProperty("next_actions", out var actions)
                && actions.ValueKind == JsonValueKind.Array
                    ? actions.EnumerateArray().Select(static item => item.Clone()).ToArray()
                    : [];

            throw new TavilyKeylessLimitException(
                GetString(error, "message") ?? "Tavily keyless usage limit exceeded.",
                code.GetString(),
                GetString(error, "window"),
                GetInt32(error, "retry_after_seconds"),
                nextActions
            );
        }

        var detail =
            document?.RootElement.TryGetProperty("detail", out var detailElement) == true
                ? detailElement
                : default;
        var message =
            detail.ValueKind == JsonValueKind.Object ? GetString(detail, "error")
            : detail.ValueKind == JsonValueKind.String ? detail.GetString()
            : null;
        message ??= string.IsNullOrWhiteSpace(body)
            ? $"Tavily {path} request failed with HTTP {(int)response.StatusCode}."
            : body;

        throw response.StatusCode switch
        {
            System.Net.HttpStatusCode.BadRequest => new BadRequestException(message),
            System.Net.HttpStatusCode.Unauthorized => new InvalidApiKeyException(message),
            System.Net.HttpStatusCode.Forbidden => new ForbiddenException(message),
            (System.Net.HttpStatusCode)432 => new ForbiddenException(message),
            (System.Net.HttpStatusCode)433 => new ForbiddenException(message),
            (System.Net.HttpStatusCode)429 => new UsageLimitExceededException(message),
            _ => new HttpRequestException(message),
        };
    }

    private static JsonDocument? TryParse(string body)
    {
        try
        {
            return JsonDocument.Parse(body);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var value)
        && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static int? GetInt32(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var value) && value.TryGetInt32(out var result)
            ? result
            : null;
}
