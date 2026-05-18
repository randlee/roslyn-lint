namespace Roslyn.Lint.Tests.Commands;

using System.ComponentModel;
using FluentAssertions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class CheckCommandTests
{
    [Fact]
    public async Task Check_WithJson_ReturnsStableEnvelope()
    {
        var application = new CliApplication(
            checkOperation: new StubCheckOperation(
                new CheckResult("/repo", "Release", "pass", [new WorkflowStepResult("build", "dotnet", "build", 0, "pass")])));

        var result = await CliTestHost.InvokeAsync(application, "check", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("check");
        envelope["data"]!["steps"]!.AsArray().Should().HaveCount(1);
    }

    [Fact]
    public async Task Check_WithJson_BackendFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            checkOperation: new ThrowingCheckOperation(
                new DotnetCommandFailedException("build", new DotnetCommandResult("/repo", ["build"], 1, string.Empty, "boom"))));

        var result = await CliTestHost.InvokeAsync(application, "check", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("backend_failure");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.BACKEND_EXEC_FAILURE");
    }

    [Fact]
    public async Task Check_WithJson_CapabilityFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            checkOperation: new ThrowingCheckOperation(
                new DotnetToolUnavailableException(new Win32Exception("missing dotnet"))));

        var result = await CliTestHost.InvokeAsync(application, "check", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("capability");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.CAPABILITY_ERROR");
    }

    private sealed class StubCheckOperation(CheckResult result) : ICheckOperation
    {
        public Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }

    private sealed class ThrowingCheckOperation(Exception exception) : ICheckOperation
    {
        public Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken)
            => Task.FromException<CheckResult>(exception);
    }
}
