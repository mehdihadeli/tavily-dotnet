# Quickstart

Install the `Tavily.Net` package, create a client, and send a typed search request.

## Install

```bash
dotnet add package Tavily.Net --prerelease
```

The package targets .NET 10, .NET Framework 4.7.2, and .NET Standard 2.0.

## Create a client

Pass an `HttpClient` and your Tavily API key:

```csharp
using Tavily.Net;

using var httpClient = new HttpClient();
var client = new TavilyClient(httpClient, "tvly-YOUR_API_KEY");
```

## Search

```csharp
var response = await client.SearchAsync(new TavilySearchRequest
{
  Query = "latest .NET release",
  SearchDepth = "advanced",
  MaxResults = 5,
  IncludeAnswer = "advanced",
  IncludeRawContent = "markdown",
}, cancellationToken);

foreach (var result in response.Results)
{
  Console.WriteLine($"{result.Title}: {result.Url}");
}
```

Every API method is asynchronous and accepts an optional `CancellationToken`.

## Next steps

- Configure [authentication](/guide/authentication).
- Review [API capabilities](/guide/capabilities).
- Add [AI tool wrappers](/guide/ai-tools) to an agent workflow.
