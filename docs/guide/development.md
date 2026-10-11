# Build and test

The repository uses a .NET 10 SDK and an `.slnx` solution file.

## Build

```bash
dotnet build Tavily.Net.slnx
```

## Unit tests

```bash
dotnet test --project tests/unit-tests/Tavily.UnitTests.csproj
```

Unit tests cover HTTP handlers, serialization, authentication, wrappers, and
typed exceptions.

## Integration tests

```bash
dotnet test --project tests/integration-tests/Tavily.IntegrationTests.csproj
```

Integration tests use `TAVILY_API_KEY` from the environment or a local `.env`
file. Without a key, tests that require live Tavily access are skipped. Live
calls consume credits and some administrative operations require additional
Tavily plan or organization permissions.
