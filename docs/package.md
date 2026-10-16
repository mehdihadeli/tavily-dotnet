# Tavily .NET package

`Tavily.Net` is a community-maintained .NET client for the Tavily API, not an
official Tavily package. The public namespace
is `Tavily.Net`.

## Installation

```bash
dotnet add package Tavily.Net --prerelease
```

The package targets `netstandard2.0`, `net472`, and `net10.0`.

## Authentication

Provide an API key explicitly:

```csharp
using Tavily.Net;

using var httpClient = new HttpClient();
var client = new TavilyClient(httpClient, "tvly-YOUR_API_KEY");
```

The client can also resolve `TAVILY_API_KEY` from the process environment or a
`.env` file. Explicit constructor values take precedence.

```text
TAVILY_API_KEY=tvly-YOUR_API_KEY
```

```csharp
var client = new TavilyClient(new HttpClient());
```

The `.env` file is searched from the current directory through its parents.
Never commit API keys; add `.env` to `.gitignore`. Search and extraction also
support Tavily keyless mode. Other operations require an API key.

## API operations

`ITavilyClient` and `TavilyClient` provide asynchronous, cancellation-aware
methods for:

- Search: `SearchAsync`
- Extract: `ExtractAsync`
- Crawl: `CrawlAsync`
- Map: `MapAsync`
- Research submission and polling: `ResearchAsync` and `GetResearchAsync`
- Usage: `GetUsageAsync`
- Request logs: `GetLogsAsync`
- Organization usage: `GetOrganizationUsageAsync`
- Feedback: `SubmitFeedbackAsync`
- Search context: `GetSearchContextAsync`
- Question-answer search: `QnaSearchAsync`
- Company information aggregation: `GetCompanyInfoAsync`

Request and response types are strongly typed. JSON property names match the
Tavily API's snake-case contract.

### Search

```csharp
var response = await client.SearchAsync(new TavilySearchRequest
{
  Query = "latest .NET release",
  SearchDepth = "advanced",
  MaxResults = 5,
  IncludeAnswer = true,
  IncludeRawContent = true,
  IncludeImages = true,
});

foreach (var result in response.Results)
{
  Console.WriteLine($"{result.Title}: {result.Url}");
}
```

Search supports depth, topic, time range, date filters, result limits, domain
filters, answer modes, raw content, images, image descriptions, favicons,
country, language, automatic parameters, exact matching, safe search, usage,
and chunk limits.

### Extract, crawl, map, and research

```csharp
var extracted = await client.ExtractAsync(new TavilyExtractRequest
{
  Urls = new[] { "https://example.com" },
  IncludeImages = true,
});

var crawl = await client.CrawlAsync(new TavilyCrawlRequest
{
  Url = "https://example.com",
  MaxDepth = 2,
  MaxBreadth = 10,
});

var map = await client.MapAsync(new TavilyMapRequest
{
  Url = "https://example.com",
});
```

These operations expose the documented depth, breadth, limits, path and domain
filters, external-link handling, extraction formats, images, favicons,
timeouts, usage reporting, and chunk controls.

Research supports model selection, streaming, structured output schemas,
citations, domain filters, output length, files, and asynchronous polling.

## Microsoft.Extensions.AI tools

The package provides `AIFunction` wrappers for AI-agent tool calling:

```csharp
using Microsoft.Extensions.AI;

AIFunction searchTool = client.AsSearchTool(
  maxResults: 5,
  searchDepth: "basic",
  includeAnswer: true);

AIFunction extractTool = client.AsExtractTool();
```

The wrappers use the same authenticated client and return typed Tavily results.

## Custom HTTP clients

Inject an `HttpClient` to configure timeout, proxy, handler, transport, or a
custom API base address:

```csharp
using var httpClient = new HttpClient
{
  BaseAddress = new Uri("https://api.tavily.com/"),
  Timeout = TimeSpan.FromSeconds(90),
};

var client = new TavilyClient(httpClient, "tvly-YOUR_API_KEY");
```

The default base address is `https://api.tavily.com/`. The SDK sends the key as
a bearer token.

## Errors and cancellation

All API methods are asynchronous and accept an optional `CancellationToken`.
The package provides typed exceptions for common failures, including:

- `MissingApiKeyException`
- `InvalidApiKeyException`
- `BadRequestException`
- `ForbiddenException`
- `UsageLimitExceededException`
- `TavilyKeylessLimitException`
- `TavilyTimeoutException`
- `KeylessUnsupportedEndpointException`

## API reference

Coverage follows the official [Tavily OpenAPI specification](https://docs.tavily.com/documentation/api-reference/openapi.json)
and the [Tavily API reference](https://docs.tavily.com/documentation/api-reference).
