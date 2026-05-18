namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Xunit;

public sealed class ViewToolsCommandTests
{
    [Fact]
    public async Task ViewTools_WithJson_ReturnsStableEnvelopeAndRegisteredTools()
    {
        var result = await CliTestHost.InvokeAsync("view", "tools", "--json");

        result.ExitCode.Should().Be(0);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeTrue();
        envelope["command"]!.GetValue<string>().Should().Be("view.tools");
        envelope["data"]!["target"]!.GetValue<string>().Should().Be("tools");

        var tools = envelope["data"]!["tools"]!.AsArray();
        tools.Should().ContainSingle();
        tools[0]!["id"]!.GetValue<string>().Should().Be("demagic");
        tools[0]!["capabilities"]!.AsArray().Select(node => node!.GetValue<string>())
            .Should().Contain("lint.demagic");
    }
}
