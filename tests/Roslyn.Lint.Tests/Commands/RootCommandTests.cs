namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Xunit;

public sealed class RootCommandTests
{
    [Fact]
    public async Task UnknownCommand_WithJson_ReturnsTypedUsageEnvelope()
    {
        var result = await CliTestHost.InvokeAsync("unknown", "--json");

        result.ExitCode.Should().Be(1);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["command"]!.GetValue<string>().Should().Be("root");
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("usage");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.USAGE_ERROR");
    }

    [Fact]
    public async Task MissingViewSubcommand_WithJson_ReturnsTypedUsageEnvelope()
    {
        var result = await CliTestHost.InvokeAsync("view", "--json");

        result.ExitCode.Should().Be(1);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["command"]!.GetValue<string>().Should().Be("view");
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("usage");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.USAGE_ERROR");
    }
}
