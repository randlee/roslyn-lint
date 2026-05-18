namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.DeMagic.Lint;
using Xunit;

public sealed class LintRoslynDeMagicCommandTests
{
    [Fact]
    public async Task LintDemagic_WithJson_ReturnsPassEnvelope()
    {
        var result = await CliTestHost.InvokeAsync(
            "lint",
            "demagic",
            "--path",
            GetFixturePath("Pass"),
            "--json");

        result.ExitCode.Should().Be(0);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeTrue();
        envelope["command"]!.GetValue<string>().Should().Be("lint.demagic");
        envelope["data"]!["tool"]!.GetValue<string>().Should().Be("demagic");
        envelope["data"]!["status"]!.GetValue<string>().Should().Be("pass");
        envelope["data"]!["findingCount"]!.GetValue<int>().Should().Be(0);
        envelope["data"]!["findings"]!.AsArray().Should().BeEmpty();
    }

    [Fact]
    public async Task LintDemagic_WithJson_ReturnsFindingsUnderData()
    {
        var result = await CliTestHost.InvokeAsync(
            "lint",
            "demagic",
            "--path",
            GetFixturePath("Findings"),
            "--json");

        result.ExitCode.Should().Be(0);

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeTrue();
        envelope["command"]!.GetValue<string>().Should().Be("lint.demagic");
        envelope["findings"].Should().BeNull();
        envelope["data"]!["status"]!.GetValue<string>().Should().Be("findings");
        envelope["data"]!["findingCount"]!.GetValue<int>().Should().Be(1);
        envelope["data"]!["findings"]![0]!["diagnosticId"]!.GetValue<string>().Should().Be("DM002");
    }

    [Fact]
    public async Task LintFast_WithJson_UsesDemagicSmokePath()
    {
        var result = await CliTestHost.InvokeAsync(
            "lint",
            "fast",
            "--path",
            GetFixturePath("Pass"),
            "--json");

        result.ExitCode.Should().Be(0);

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeTrue();
        envelope["command"]!.GetValue<string>().Should().Be("lint.fast");
        envelope["data"]!["tool"]!.GetValue<string>().Should().Be("demagic");
    }

    [Fact]
    public async Task LintDemagic_WithMissingPath_ReturnsUsageEnvelope()
    {
        var result = await CliTestHost.InvokeAsync(
            "lint",
            "demagic",
            "--path",
            "/path/that/does/not/exist",
            "--json");

        result.ExitCode.Should().Be(1);

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["command"]!.GetValue<string>().Should().Be("lint.demagic");
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("usage");
        envelope["error"]!["details"]!["path"]!.GetValue<string>().Should().Be("/path/that/does/not/exist");
    }

    [Fact]
    public async Task LintDemagic_InternalFailure_NormalizesToCliError()
    {
        var application = new CliApplication([new RoslynDeMagicToolModule(new ThrowingLintHandler())]);
        var result = await CliTestHost.InvokeAsync(
            application,
            "lint",
            "demagic",
            "--path",
            GetFixturePath("Pass"),
            "--json");

        result.ExitCode.Should().Be(1);

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["command"]!.GetValue<string>().Should().Be("lint.demagic");
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("backend_failure");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.BACKEND_EXEC_FAILURE");
        envelope["error"]!["details"]!["tool"]!.GetValue<string>().Should().Be("demagic");
    }

    private static string GetFixturePath(string name)
        => Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "TestData",
            "Lint",
            name));

    private sealed class ThrowingLintHandler : ILintToolCommandHandler<LintToolRequest, LintToolResult>
    {
        public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
            => throw new InvalidOperationException("boom");
    }
}
