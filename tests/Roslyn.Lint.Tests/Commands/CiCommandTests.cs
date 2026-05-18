namespace Roslyn.Lint.Tests.Commands;

using System.ComponentModel;
using FluentAssertions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class CiCommandTests
{
    [Fact]
    public async Task Ci_WithJson_ReturnsStableEnvelope()
    {
        var application = new CliApplication(
            ciOperation: new StubCiOperation(
                new CiResult(
                    "/repo",
                    "Release",
                    "pass",
                    new LintProfileResult("ci", "/repo", "pass", 0, ["demagic"], []),
                    [new WorkflowStepResult("test", "dotnet", "test", 0, "pass")])));

        var result = await CliTestHost.InvokeAsync(application, "ci", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("ci");
        envelope["data"]!["lint"]!["profile"]!.GetValue<string>().Should().Be("ci");
    }

    [Fact]
    public async Task Ci_WithJson_BackendFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            ciOperation: new ThrowingCiOperation(
                new DotnetCommandFailedException("test", new DotnetCommandResult("/repo", ["test"], 1, string.Empty, "boom"))));

        var result = await CliTestHost.InvokeAsync(application, "ci", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("backend_failure");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.BACKEND_EXEC_FAILURE");
    }

    [Fact]
    public async Task Ci_WithJson_CapabilityFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            ciOperation: new ThrowingCiOperation(
                new DotnetToolUnavailableException(new Win32Exception("missing dotnet"))));

        var result = await CliTestHost.InvokeAsync(application, "ci", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("capability");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.CAPABILITY_ERROR");
    }

    private sealed class StubCiOperation(CiResult result) : ICiOperation
    {
        public Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }

    private sealed class ThrowingCiOperation(Exception exception) : ICiOperation
    {
        public Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken)
            => Task.FromException<CiResult>(exception);
    }
}
