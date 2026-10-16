using System.Text.Json;
using Microsoft.Extensions.AI;
using Shouldly;
using Tavily.Net;
using Xunit;

namespace AgentSkillsMcp.Search.Tests;

public sealed class TavilyAiToolIntegrationTests : TavilyIntegrationTestBase
{
    public static new bool LiveTestsEnabled => TavilyIntegrationTestBase.LiveTestsEnabled;

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Search_tool_invokes_live_tavily_search()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var tool = tavily.AsSearchTool(maxResults: 1, includeAnswer: false);

        var result = await tool.InvokeAsync(
            new AIFunctionArguments { ["query"] = "Microsoft .NET official documentation" },
            cancellationToken
        );

        var json = result.ShouldBeOfType<JsonElement>();
        json.GetProperty("results").GetArrayLength().ShouldBeGreaterThan(0);
        json.GetProperty("results")[0].GetProperty("url").GetString()
            .ShouldNotBeNullOrWhiteSpace();
    }

    [Fact(
        Skip = "Provide TAVILY_API_KEY in .env or the process environment to run live tests.",
        SkipUnless = nameof(LiveTestsEnabled)
    )]
    public async Task Extract_tool_invokes_live_tavily_extraction()
    {
        var (tavily, cancellationToken) = CreateLiveClient();
        var tool = tavily.AsExtractTool();

        var result = await tool.InvokeAsync(
            new AIFunctionArguments
            {
                ["url"] = "https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview",
            },
            cancellationToken
        );

        var json = result.ShouldBeOfType<JsonElement>();
        json.GetProperty("results").GetArrayLength().ShouldBeGreaterThan(0);
        json.GetProperty("results")[0].GetProperty("raw_content").GetString()
            .ShouldNotBeNullOrWhiteSpace();
    }
}