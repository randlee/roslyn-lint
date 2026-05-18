namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class LintProfileCommandTests
{
    [Fact]
    public async Task LintFull_WithJson_ReturnsProfileEnvelope()
    {
        var application = new CliApplication(
            lintToolOperation: new StubLintToolOperation(
                new LintToolResult("demagic", "/repo", "pass", 0, [])));

        var result = await CliTestHost.InvokeAsync(application, "lint", "full", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("lint.full");
        envelope["data"]!["profile"]!.GetValue<string>().Should().Be("full");
        envelope["data"]!["members"]!.AsArray().Should().ContainSingle();
    }

    [Fact]
    public async Task LintCi_WithJson_ReturnsProfileEnvelope()
    {
        var application = new CliApplication(
            lintToolOperation: new StubLintToolOperation(
                new LintToolResult("demagic", "/repo", "pass", 0, [])));

        var result = await CliTestHost.InvokeAsync(application, "lint", "ci", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("lint.ci");
        envelope["data"]!["members"]!.AsArray()[0]!.GetValue<string>().Should().Be("demagic");
    }

    private sealed class StubLintToolOperation(LintToolResult result) : ILintToolOperation
    {
        public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath });
    }
}
