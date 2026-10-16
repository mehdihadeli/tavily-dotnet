# Samples

Runnable examples for the `Tavily.Net` and `Tavily.Net.Extensions.AI` packages.

## Run a sample

From the repository root:

```bash
dotnet run --project samples/Tavily.Net.Samples -- search
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- search-tool
```

The client reads `TAVILY_API_KEY` from the environment or a local `.env` file.
Search and extract also support Tavily keyless mode. Never commit API keys.

```powershell
$env:TAVILY_API_KEY = "tvly-YOUR_API_KEY"
$env:TAVILY_PROJECT_ID = "project-id"
$env:TAVILY_ORGANIZATION_NAME = "organization-name"
$env:TAVILY_REQUEST_ID = "request-id"
```

## Available commands

| Command | Demonstrates |
| --- | --- |
| `search` | Typed web search with answer and sources |
| `extract` | Extract readable content from URLs |
| `crawl` | Crawl a site with depth and result limits |
| `map` | Map pages in a site |
| `research` | Start research and poll for its result |
| `research-stream` | Consume streaming research output |
| `usage` | Read account or project usage |
| `logs` | Read filtered request logs |
| `organization-usage` | Read organization usage aggregates |
| `feedback` | Submit feedback for a request |
| `company` | Run the company-information helper |
| `context` | Build serialized search context for RAG |
| `qna` | Ask Tavily for a direct answer |

The core sample uses `Spectre.Console.Cli` and accepts the operation as a
command argument. Run `dotnet run --project samples/Tavily.Net.Samples -- --help`
to see the generated command help.

## Microsoft.Extensions.AI sample

The AI package has its own sample project so applications that only use the
core package do not need to reference the optional integration:

```bash
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- search-tool --query "latest .NET release"
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- extract-tool --url https://learn.microsoft.com/dotnet/
```

These commands create `AIFunction` instances with `AsSearchTool()` and
`AsExtractTool()`, then invoke them using `AIFunctionArguments`.

Most commands make live API requests and may consume credits. Administrative
commands use placeholder environment values in the source; replace them with
values from your Tavily account before running.
