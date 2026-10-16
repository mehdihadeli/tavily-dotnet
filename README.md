# Tavily .NET SDK

[![build-test-publish](https://img.shields.io/github/actions/workflow/status/mehdihadeli/tavily-dotnet/build-and-publish.yml?branch=main&style=for-the-badge&label=build-test-publish)](https://github.com/mehdihadeli/tavily-dotnet/actions/workflows/build-and-publish.yml)
[![License](https://img.shields.io/github/license/mehdihadeli/tavily-dotnet?style=for-the-badge)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET%20Standard%202.0%20%7C%20.NET%20Framework%204.7.2%20%7C%20.NET%2010-512bd4?style=for-the-badge)](https://dotnet.microsoft.com/platform/dotnet)

[![Tavily.Net](https://img.shields.io/nuget/v/Tavily.Net?style=for-the-badge&color=blue&label=Tavily.Net)](https://www.nuget.org/packages/Tavily.Net)
[![Tavily.Net.Extensions.AI](https://img.shields.io/nuget/v/Tavily.Net.Extensions.AI?style=for-the-badge&color=blue&label=Tavily.Net.Extensions.AI)](https://www.nuget.org/packages/Tavily.Net.Extensions.AI)

**Tavily .NET SDK** is a community-maintained .NET client for Tavily search,
extraction, crawling, mapping, research, usage, logs, and feedback APIs.
Requests and responses are strongly typed and all operations are asynchronous.

The libraries target .NET Standard 2.0, .NET Framework 4.7.2, and .NET 10.
Install the core package for native Tavily API calls, or add the optional AI
package for `Microsoft.Extensions.AI` tool interoperability.

> [!NOTE]
> This is a community-maintained SDK, not an official Tavily package. API
> coverage follows the official [Tavily OpenAPI specification](https://docs.tavily.com/documentation/api-reference/openapi.json).

## How It Works

1. Provide a Tavily API key through options, the `TAVILY_API_KEY` environment
  variable, or a local `.env` file.
2. Create a `TavilyClient` with an injected or SDK-created `HttpClient`.
3. Send a typed request such as `TavilySearchRequest` or
  `TavilyExtractRequest`.
4. Consume the typed response, including results, citations, usage, request IDs,
  and endpoint-specific metadata.

> [!IMPORTANT]
> Requests are sent to Tavily's configured API endpoint. The SDK does not run
> models locally. Review request state and avoid sending credentials or
> unnecessary personal data.

## Packages

| Package | Description | Latest NuGet |
| --- | --- | --- |
| [`Tavily.Net`](https://www.nuget.org/packages/Tavily.Net) | Core typed client for Tavily search, extraction, crawling, mapping, research, usage, logs, and feedback APIs. | [![Tavily.Net](https://img.shields.io/nuget/v/Tavily.Net?style=for-the-badge&color=blue&label=Tavily.Net)](https://www.nuget.org/packages/Tavily.Net) |
| [`Tavily.Net.Extensions.AI`](https://www.nuget.org/packages/Tavily.Net.Extensions.AI) | Optional `Microsoft.Extensions.AI` integration exposing `AIFunction` tools for Tavily search and extraction. | [![Tavily.Net.Extensions.AI](https://img.shields.io/nuget/v/Tavily.Net.Extensions.AI?style=for-the-badge&color=blue&label=Tavily.Net.Extensions.AI)](https://www.nuget.org/packages/Tavily.Net.Extensions.AI) |

Install `Tavily.Net` for direct typed API access. Add
`Tavily.Net.Extensions.AI` when an application uses
`Microsoft.Extensions.AI` tool-calling pipelines. The AI package builds on the
core client and does not replace it.

Each package has its own NuGet README:

- [Core SDK README](src/Tavily.Net/README.md)
- [Microsoft.Extensions.AI README](src/Tavily.Net.Extensions.AI/README.md)

## Samples

Runnable examples for every client capability are in [samples/README.md](samples/README.md).
The core sample includes commands for search, extraction, crawling, mapping,
research polling and streaming, usage, logs, organization usage, feedback,
company information, search context, and Q&A. A separate sample project covers
the optional `Microsoft.Extensions.AI` tools.

Run an example from the repository root:

```bash
dotnet run --project samples/Tavily.Net.Samples -- search
```

For the AI integration sample:

```bash
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- search-tool
```

Most samples call the live Tavily API and read `TAVILY_API_KEY` from the
environment or a local `.env` file. See the samples guide for the complete
command list and required environment variables.

## Installation

Install the core package for direct typed Tavily API access:

```bash
dotnet add package Tavily.Net
```

Add the optional integration package only when the application uses
`Microsoft.Extensions.AI`:

```bash
dotnet add package Tavily.Net.Extensions.AI
```

For unreleased versions, append `--prerelease` to either command. Both package
identities are published separately; the AI package depends on the core package.

## Quick Start

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

The package identity and public namespace are both `Tavily.Net`.

### Microsoft.Extensions.AI tools

Install the companion package when using Tavily as an AI function tool:

```bash
dotnet add package Tavily.Net.Extensions.AI
```

Then create tools from the same `TavilyClient`:

```csharp
using Microsoft.Extensions.AI;

AIFunction searchTool = client.AsSearchTool();
AIFunction extractTool = client.AsExtractTool();
```

The AI package depends on `Tavily.Net` and adds no replacement client. It keeps
`Microsoft.Extensions.AI` optional for applications that do not use tool
calling.

## ✨ Features

- **API coverage:** Search, Extract, Crawl, Map, Research, usage, logs, and
  organization usage, plus feedback and company-information helpers.
- **AI integration:** Optional `Microsoft.Extensions.AI` search and extraction
  tools through `Tavily.Net.Extensions.AI`.
- **Typed operations:** Request and response models, snake-case JSON,
  cancellation tokens, and typed API exceptions.
- **Flexible configuration:** Injected `HttpClient`, custom transport and base
  address, and API keys from constructors, environment variables, or `.env`.
- **Framework support:** .NET 10, .NET Framework 4.7.2, and .NET Standard 2.0.

## 🔐 Authentication

The SDK accepts a Tavily API key for all operations, or can run in keyless mode
for `SearchAsync` and `ExtractAsync`. Pass the key directly, set
`TAVILY_API_KEY` in the process environment, or place it in a `.env` file.
Explicit constructor values take precedence over environment values.

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

In keyless mode, only search and extraction are available. Other operations
throw `KeylessUnsupportedEndpointException` before sending a request.

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
  IncludeImages = true,
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

The .NET client exposes polling through `GetResearchAsync` and streaming through
`StreamResearchAsync`:

```csharp
await foreach (var chunk in client.StreamResearchAsync(
  new TavilyResearchRequest
  {
    Input = "Research the latest developments in AI",
  },
  cancellationToken))
{
  Console.WriteLine(chunk);
}
```

Use `StreamResearchAsync` instead of setting `TavilyResearchRequest.Stream`
when consuming streaming research responses.

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

## Development and Tests

Tests use xUnit v3 with the Microsoft.Testing.Platform runner and Shouldly
assertions. Shared test settings live in `tests/Directory.Build.props`.

The test layout is split by package and test type:

- `tests/Tavily.Net/UnitTests` contains deterministic core client tests.
- `tests/Tavily.Net/IntegrationTests` contains live core API checks.
- `tests/Tavily.Net.Extensions.AI/UnitTests` contains deterministic AI adapter tests.
- `tests/Tavily.Net.Extensions.AI/IntegrationTests` contains live AI tool checks.

Build the complete solution with:

```bash
dotnet build Tavily.Net.slnx --configuration Release
```

Run the core unit tests with:

```bash
dotnet test --project tests/Tavily.Net/UnitTests/Tavily.Net.UnitTests.csproj
```

Run the AI unit tests with:

```bash
dotnet test --project tests/Tavily.Net.Extensions.AI/UnitTests/Tavily.Net.Extensions.AI.UnitTests.csproj
```

Run live integration tests with:

```bash
dotnet test --project tests/Tavily.Net/IntegrationTests/Tavily.Net.IntegrationTests.csproj
dotnet test --project tests/Tavily.Net.Extensions.AI/IntegrationTests/Tavily.Net.Extensions.AI.IntegrationTests.csproj
```

Integration tests require `TAVILY_API_KEY` and may spend API credits. Without
the key, live tests are skipped.

Pack both packages locally without publishing them:

```bash
dotnet pack Tavily.Net.slnx --configuration Release --output artifacts
```

The solution also contains the runnable project under
`samples/Tavily.Net.Samples`.

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
