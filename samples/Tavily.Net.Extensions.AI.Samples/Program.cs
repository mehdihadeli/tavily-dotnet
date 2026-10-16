using System.Text.Json;
using Microsoft.Extensions.AI;
using Spectre.Console.Cli;
using Tavily.Net;

var app = new CommandApp<AiToolsCommand>();
app.Configure(config =>
{
    config.SetApplicationName("tavily-ai-samples");
    config.AddExample("search-tool");
    config.AddExample("extract-tool");
});

return app.Run(args);

public sealed class AiToolSettings : CommandSettings
{
    [CommandArgument(0, "<operation>")]
    public string Operation { get; set; } = "search-tool";

    [CommandOption("--query <QUERY>")]
    public string Query { get; set; } = "latest .NET release";

    [CommandOption("--url <URL>")]
    public string Url { get; set; } = "https://learn.microsoft.com/dotnet/";
}

public sealed class AiToolsCommand : AsyncCommand<AiToolSettings>
{
    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        AiToolSettings settings,
        CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(90) };
        ITavilyClient client = new TavilyClient(httpClient);
        var searchTool = client.AsSearchTool(maxResults: 5, searchDepth: "basic");
        var extractTool = client.AsExtractTool();

        try
        {
            object? result = settings.Operation.ToLowerInvariant() switch
            {
                "search-tool" => await searchTool.InvokeAsync(
                    new AIFunctionArguments { ["query"] = settings.Query }, cancellationToken),
                "extract-tool" => await extractTool.InvokeAsync(
                    new AIFunctionArguments { ["url"] = settings.Url }, cancellationToken),
                _ => throw new InvalidOperationException(
                    "Use search-tool or extract-tool.")
            };

            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
            }));
            return 0;
        }
        catch (TavilyException exception)
        {
            Console.Error.WriteLine($"Tavily request failed: {exception.Message}");
            return 1;
        }
    }
}
