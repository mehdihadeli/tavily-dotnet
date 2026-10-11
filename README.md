# 🔎 Tavily .NET SDK

[![NuGet](https://img.shields.io/nuget/vpre/Tavily.Net.svg)](https://www.nuget.org/packages/Tavily.Net)
[![Build and test](https://github.com/mehdihadeli/tavily-dotnet/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/mehdihadeli/tavily-dotnet/actions/workflows/build-and-publish.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Use Tavily search, extraction, crawling, mapping, research, and usage APIs from
.NET applications with strongly typed requests and responses.

The package supports .NET 10, .NET Framework 4.7.2, and .NET Standard 2.0. It
also provides `Microsoft.Extensions.AI` tools for search and extraction.

> [!NOTE]
> `Tavily.Net` is a community-maintained SDK, not an official Tavily package.

API coverage follows the official [Tavily OpenAPI specification](https://docs.tavily.com/documentation/api-reference/openapi.json).

## 🚀 Quick Start

Install the preview package:

```bash
dotnet add package Tavily.Net --prerelease
```

Create a client and search:

```csharp
using Tavily.Net;

using var httpClient = new HttpClient();
var client = new TavilyClient(httpClient, "tvly-YOUR_API_KEY");

var response = await client.SearchAsync(new TavilySearchRequest
{
  Query = "latest .NET release",
  MaxResults = 5,
  IncludeAnswer = "advanced",
});

foreach (var result in response.Results)
{
  Console.WriteLine($"{result.Title}: {result.Url}");
}
```

The NuGet package is `Tavily.Net`; its public namespace is
`Tavily.Net`.

## ✨ Features

- **API coverage:** Search, Extract, Crawl, Map, Research, usage, logs, and
  organization usage, plus feedback and company-information helpers.
- **AI integration:** `Microsoft.Extensions.AI` search and extraction tools,
  RAG context, and question-answer helpers.
- **Typed operations:** Request and response models, snake-case JSON,
  cancellation tokens, and typed API exceptions.
- **Flexible configuration:** Injected `HttpClient`, custom transport and base
  address, and API keys from constructors, environment variables, or `.env`.
- **Framework support:** .NET 10, .NET Framework 4.7.2, and .NET Standard 2.0.

## 🔐 Authentication

The SDK requires a Tavily API key. Pass it directly, set `TAVILY_API_KEY` in
the process environment, or place it in a `.env` file. Explicit constructor
values take precedence over environment values.

For local development, the client searches for `.env` from the current
directory upward through its parent directories:

```csharp
using var httpClient = new HttpClient();
var client = new TavilyClient(httpClient);
```

Example `.env`:

```text
TAVILY_API_KEY=tvly-YOUR_API_KEY
```

> [!WARNING]
> Never commit API keys. Add `.env` to `.gitignore` and keep real credentials
> out of source files and logs.

Unlike the Python SDK, this implementation does not support keyless mode;
constructing a client without a resolvable key throws `MissingApiKeyException`.

The client sends the API key as a bearer token and defaults to Tavily's
OpenAPI server, `https://api.tavily.com/`. Supply an `HttpClient` with a
custom base address when needed.

## 🧭 API Capabilities

| Goal | Client methods | Endpoints |
| --- | --- | --- |
| Search and extract content | `SearchAsync`, `ExtractAsync` | `POST /search`, `POST /extract` |
| Explore a website | `CrawlAsync`, `MapAsync` | `POST /crawl`, `POST /map` |
| Submit and poll research | `ResearchAsync`, `GetResearchAsync` | `POST /research`, `GET /research/{request_id}` |
| Inspect usage and activity | `GetUsageAsync`, `GetLogsAsync`, `GetOrganizationUsageAsync` | `GET /usage`, `POST /logs`, `POST /org-usage` |
| Send feedback | `SubmitFeedbackAsync` | Python SDK-compatible `POST /feedback` |

`TavilySearchRequest` supports search depth, topics, time ranges, date and
published-date filters, result limits, domain filters, domain filter mode,
answer modes, raw-content modes, images, image descriptions, favicons,
country, language, language filtering, automatic parameters, exact matching,
usage reporting, safe search, and chunk limits.

`TavilyExtractRequest`, `TavilyCrawlRequest`, and `TavilyMapRequest` support
their documented depth, breadth, limit, path/domain filters, external-link
handling, extraction format, images, favicons, timeouts, usage reporting, and
chunk controls. Research supports models, streaming, structured output
schemas, citation formats, domain filters, output length, files, and polling
with optional usage details.

Administrative operations expose API-key usage, filtered request logs, and
organization-level usage aggregates. The response models include endpoint
results, failures, images, sources, usage metrics, request IDs, masked log
metadata, organization filters, and per-key usage summaries.

## 🧰 Client Usage

All client methods are asynchronous and accept an optional
`CancellationToken`. The examples below use a token supplied by the caller:

```csharp
var search = await client.SearchAsync(new TavilySearchRequest
{
  Query = "latest .NET release",
  SearchDepth = "advanced",
  MaxResults = 5,
  IncludeAnswer = "advanced",
  IncludeRawContent = "markdown",
  IncludeUsage = true,
}, cancellationToken);

var usage = await client.GetUsageAsync(projectId: "project-id", cancellationToken);
var logs = await client.GetLogsAsync(new TavilyLogsRequest
{
  Limit = 25,
  Endpoints = ["search", "extract"],
}, cancellationToken);
```

### Search

```csharp
var response = await client.SearchAsync(new TavilySearchRequest
{
  Query = "latest .NET release",
  SearchDepth = "advanced",
  MaxResults = 5,
  IncludeAnswer = "advanced",
  IncludeRawContent = "markdown",
  IncludeImages = true,
  IncludeDomains = ["learn.microsoft.com"],
}, cancellationToken);

foreach (var result in response.Results)
{
  Console.WriteLine($"{result.Title}: {result.Url}");
}
```

`TavilySearchRequest` also supports topics, time ranges, date filters,
excluded domains, country and language filters, exact matching, safe search,
favicons, image descriptions, automatic parameters, usage reporting, and
chunk limits.

### Extract

```csharp
var response = await client.ExtractAsync(new TavilyExtractRequest
{
  Urls =
  [
    "https://learn.microsoft.com/dotnet/",
    "https://learn.microsoft.com/aspnet/core/",
  ],
  ExtractDepth = "advanced",
  Format = "markdown",
  IncludeImages = true,
}, cancellationToken);

foreach (var result in response.Results)
{
  Console.WriteLine(result.RawContent);
}
```

### Crawl and map

```csharp
var crawl = await client.CrawlAsync(new TavilyCrawlRequest
{
  Url = "https://docs.tavily.com",
  MaxDepth = 2,
  Limit = 20,
  Instructions = "Find documentation pages about search",
}, cancellationToken);

var map = await client.MapAsync(new TavilyMapRequest
{
  Url = "https://docs.tavily.com",
  MaxDepth = 2,
  Limit = 20,
}, cancellationToken);
```

### Research

Research is created asynchronously and can then be polled by request ID:

```csharp
var queued = await client.ResearchAsync(new TavilyResearchRequest
{
  Input = "Research the latest developments in AI",
  Model = "pro",
  CitationFormat = "apa",
}, cancellationToken);

var result = await client.GetResearchAsync(
  queued.RequestId!,
  cancellationToken,
  includeUsage: true);

Console.WriteLine($"Status: {result.Status}");
Console.WriteLine(result.Content);
```

The .NET client exposes polling through `GetResearchAsync`; it does not expose
the Python SDK's streaming research iterator.

### Feedback and convenience helpers

```csharp
var search = await client.SearchAsync(
  new TavilySearchRequest { Query = "latest AI research" },
  cancellationToken);

await client.SubmitFeedbackAsync(new TavilyFeedbackRequest
{
  RequestId = search.RequestId,
  AgentScore = 1,
  Comment = "Useful result",
}, cancellationToken);

var companyResults = await client.GetCompanyInfoAsync(
  "Microsoft",
  maxResults: 3,
  cancellationToken: cancellationToken);
```

`GetSearchContextAsync` returns serialized URL/content pairs for RAG prompts,
and `QnaSearchAsync` returns Tavily's answer text:

```csharp
var context = await client.GetSearchContextAsync(
  new TavilySearchContextRequest { Query = "What is .NET?" },
  cancellationToken: cancellationToken);
var answer = await client.QnaSearchAsync("What is .NET?", cancellationToken);
```

The company-information helper runs searches for news, general, and finance,
then merges, ranks, and limits the returned results.

## 🤖 AI Tools

Expose Tavily search and extraction as `Microsoft.Extensions.AI` functions:

```csharp
using Tavily.Net;
using Microsoft.Extensions.AI;

var client = new TavilyClient(httpClient, tavilyApiKey);
AIFunction searchTool = client.AsSearchTool();
AIFunction extractTool = client.AsExtractTool();
```

Configure the search tool's optional controls:

```csharp
AIFunction searchTool = client.AsSearchTool(
  maxResults: 5,
  searchDepth: "basic",
  includeAnswer: true);
```

## ⚙️ HTTP Configuration

Inject an `HttpClient` to configure the transport, proxy, handler, timeout, or
base address:

```csharp
using var httpClient = new HttpClient
{
  BaseAddress = new Uri("https://api.tavily.com/"),
  Timeout = TimeSpan.FromSeconds(90),
};

var client = new TavilyClient(httpClient, "tvly-YOUR_API_KEY");
```

The SDK always applies the resolved Tavily key as a bearer token. It does not
currently provide the Python SDK's externally-authenticated session mode or
client-level `project_id`, `session_id`, `human_id`, and `client_name` options.

## 🛡️ Error Handling

The SDK exposes typed exceptions for invalid keys, bad requests, forbidden
access, usage limits, keyless limits, missing keys, and request timeouts.
See the [error reference](docs/reference/errors.md) for exception types and
handling guidance.

## 🏗️ Build Validation

Build the complete solution, including source and test projects, with:

```bash
dotnet build Tavily.Net.slnx
```

## 🧪 Tests

Tests use xUnit v3 with the Microsoft.Testing.Platform runner and Shouldly
assertions. Shared test settings live in `tests/Directory.Build.props`.

The test layout is split into:

- `tests/unit-tests/Tavily.UnitTests.csproj` for isolated HTTP
  handler, serialization, authentication, wrapper, and exception tests.
- `tests/integration-tests/Tavily.IntegrationTests.csproj` for
  live Tavily API checks enabled when `TAVILY_API_KEY` is available.

Run unit tests with:

```bash
dotnet test --project tests/unit-tests/Tavily.UnitTests.csproj
```

Run live integration tests with:

```bash
dotnet test --project \
  tests/integration-tests/Tavily.IntegrationTests.csproj
```

Without `TAVILY_API_KEY`, integration tests are discovered but skipped so
ordinary test runs do not spend API credits.

## 📚 Documentation

| Guide | Contents |
| --- | --- |
| [Quickstart](docs/guide/quickstart.md) | Installation and first search |
| [Authentication](docs/guide/authentication.md) | API keys and `.env` resolution |
| [Capabilities](docs/guide/capabilities.md) | Search, extraction, crawling, mapping, and research |
| [AI tools](docs/guide/ai-tools.md) | `Microsoft.Extensions.AI` function wrappers |
| [Versioning](docs/guide/versioning.md) | Preview, RC, stable, and release workflow |

The [VitePress documentation](docs/index.md) contains the rendered-site
source, including the quickstart, authentication, API capabilities, AI
wrappers, transport configuration, development workflow, and reference pages.
Run the docs locally from `docs/` with:

```bash
npm install
npm run dev
```

The [package guide](docs/package.md) remains available as a single-file
overview for package consumers.

For endpoint parameters and server behavior, see the [Tavily API
reference](https://docs.tavily.com/documentation/api-reference) and the
[official OpenAPI specification](https://docs.tavily.com/documentation/api-reference/openapi.json).

## 💳 API Credits

Tavily API calls consume credits according to your Tavily plan. Review the
[Tavily credits and pricing documentation](https://docs.tavily.com/documentation/api-credits)
before running live integration tests or high-volume workflows.


## 📄 License

This project is licensed under the terms of the [MIT license](LICENSE).
