# Samples

The repository includes runnable examples for the core client and the optional
`Microsoft.Extensions.AI` integration.

## Core client

The core sample uses `Spectre.Console.Cli` and exposes one operation per
capability:

```bash
dotnet run --project samples/Tavily.Net.Samples -- search
dotnet run --project samples/Tavily.Net.Samples -- research-stream
```

Available operations include search, extraction, crawling, mapping, research,
usage, logs, organization usage, feedback, company information, search
context, and Q&A.

## Microsoft.Extensions.AI

The AI package has a separate sample project that creates and invokes
`AIFunction` adapters:

```bash
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- search-tool
dotnet run --project samples/Tavily.Net.Extensions.AI.Samples -- extract-tool
```

Most samples make live API requests and read `TAVILY_API_KEY` from the
environment or a local `.env` file. Search and extraction also support Tavily
keyless mode. Administrative examples may require additional environment
variables and account permissions.

See the [complete sample command list](https://github.com/mehdihadeli/tavily-dotnet/blob/main/samples/README.md)
for all commands and required environment variables.