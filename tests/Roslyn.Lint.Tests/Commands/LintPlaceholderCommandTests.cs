namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Xunit;

public sealed class LintPlaceholderCommandTests
{
    [Theory]
    [InlineData("demagic", "lint.demagic", "A6")]
    [InlineData("fast", "lint.fast", "A6")]
    [InlineData("full", "lint.full", "A7")]
    [InlineData("ci", "lint.ci", "A7")]
    public async Task LintPlaceholder_WithJson_ReturnsTypedCapabilityEnvelope(
        string subcommand,
        string commandId,
        string plannedSprint)
    {
        var result = await CliTestHost.InvokeAsync("lint", subcommand, "--json");

        result.ExitCode.Should().Be(1);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["command"]!.GetValue<string>().Should().Be(commandId);
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("capability");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.CAPABILITY_ERROR");
        envelope["error"]!["details"]!["planned_sprint"]!.GetValue<string>().Should().Be(plannedSprint);
        envelope["error"]!["suggested_action"]!.GetValue<string>()
            .Should().Be("Follow the sprint plan for the next implementation milestone.");
    }
}
