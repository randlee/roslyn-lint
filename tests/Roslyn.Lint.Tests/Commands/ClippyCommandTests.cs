namespace Roslyn.Lint.Tests.Commands;

using System.ComponentModel;
using FluentAssertions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class ClippyCommandTests
{
    [Fact]
    public async Task Clippy_WithJson_ReturnsStableEnvelope()
    {
        var application = new CliApplication(
            clippyOperation: new StubClippyOperation(
                new ClippyResult("/repo", "Release", "pass", [new WorkflowStepResult("format", "dotnet", "format", 0, "pass")])));

        var result = await CliTestHost.InvokeAsync(application, "clippy", "--json");

        result.ExitCode.Should().Be(0);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["command"]!.GetValue<string>().Should().Be("clippy");
        envelope["data"]!["status"]!.GetValue<string>().Should().Be("pass");
    }

    [Fact]
    public async Task Clippy_WithJson_BackendFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            clippyOperation: new ThrowingClippyOperation(
                new DotnetCommandFailedException("format", new DotnetCommandResult("/repo", ["format"], 2, string.Empty, "boom"))));

        var result = await CliTestHost.InvokeAsync(application, "clippy", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("backend_failure");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.BACKEND_EXEC_FAILURE");
    }

    [Fact]
    public async Task Clippy_WithJson_CapabilityFailure_ReturnsTypedErrorEnvelope()
    {
        var application = new CliApplication(
            clippyOperation: new ThrowingClippyOperation(
                new DotnetToolUnavailableException(new Win32Exception("missing dotnet"))));

        var result = await CliTestHost.InvokeAsync(application, "clippy", "--json");

        result.ExitCode.Should().Be(1);
        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeFalse();
        envelope["error"]!["kind"]!.GetValue<string>().Should().Be("capability");
        envelope["error"]!["code"]!.GetValue<string>().Should().Be("CLI.CAPABILITY_ERROR");
    }

    private sealed class StubClippyOperation(ClippyResult result) : IClippyOperation
    {
        public Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }

    private sealed class ThrowingClippyOperation(Exception exception) : IClippyOperation
    {
        public Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken)
            => Task.FromException<ClippyResult>(exception);
    }
}
