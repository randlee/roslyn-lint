namespace sc.lint.roslyn.tests.commands;

using FluentAssertions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.operations;
using Xunit;

public sealed class LintProfileCommandTests
{
    [Fact]
    public async Task LintFast_WithJson_ReturnsProfileEnvelope()
    {
        var application = new CliApplication(
            lintToolOperation: new StubLintToolOperation(
                new LintToolResult("demagic", "/repo", "pass", 0, [])));

        var result = await CliTestHost.InvokeAsync(application, "lint", "fast", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("lint.fast");
        envelope["data"]!["profile"]!.GetValue<string>().Should().Be("fast");
        envelope["data"]!["members"]!.AsArray().Should().ContainSingle(m => m!.GetValue<string>() == "demagic");
    }

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
        envelope["data"]!["members"]!.AsArray().Should().ContainSingle(m => m!.GetValue<string>() == "demagic");
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
