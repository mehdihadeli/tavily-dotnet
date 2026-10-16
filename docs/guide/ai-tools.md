# AI tool wrappers

The companion `Tavily.Net.Extensions.AI` package provides
`Microsoft.Extensions.AI` `AIFunction` wrappers for search and extraction. It
depends on `Tavily.Net` and uses the same authenticated `TavilyClient`.

Install the optional integration package. It depends on the core package:

```bash
dotnet add package Tavily.Net.Extensions.AI --prerelease
```

```csharp
using Microsoft.Extensions.AI;

AIFunction searchTool = client.AsSearchTool();
AIFunction extractTool = client.AsExtractTool();
```

Set common search controls while creating the tool:

```csharp
AIFunction searchTool = client.AsSearchTool(
  maxResults: 5,
  searchDepth: "basic",
  includeAnswer: true);
```

Pass the resulting functions to the agent or chat client that supports
`AIFunction` tools. Tool execution remains subject to the same API key,
timeout, cancellation, and Tavily account limits as direct client calls.

See the [samples guide](/guide/samples) for runnable search and extraction
tool examples.
