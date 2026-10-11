using Tavily.Net;
using Shouldly;
using Xunit;

public sealed class TavilyConfigurationTests : TavilyUnitTestBase
{
    [Fact]
    public void Constructor_rejects_missing_api_key_with_tavily_exception()
    {
        var exception = Should.Throw<MissingApiKeyException>(
            () => new TavilyClient(new HttpClient(new StubHandler("{}")), " ")
        );

        exception.ParamName.ShouldBe("apiKey");
    }

    [Fact]
    public async Task Constructor_reads_api_key_from_env_file()
    {
        var envFile = Path.Combine(Path.GetTempPath(), $"tavily-{Guid.NewGuid():N}.env");
        await File.WriteAllTextAsync(envFile, "OTHER=value\nexport TAVILY_API_KEY=env-file-key\n");

        try
        {
            using var client = CreateClient(
                "{\"query\":\"dotnet\",\"results\":[]}",
                request =>
                {
                    request.Headers.Authorization!.Parameter.ShouldBe("env-file-key");
                    return Task.CompletedTask;
                }
            );
            var tavily = new TavilyClient(client, envFilePath: envFile);

            await tavily.SearchAsync(new TavilySearchRequest { Query = "dotnet" });
        }
        finally
        {
            File.Delete(envFile);
        }
    }
}