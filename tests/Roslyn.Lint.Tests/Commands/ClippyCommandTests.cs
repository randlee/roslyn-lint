namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
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

    private sealed class StubClippyOperation(ClippyResult result) : IClippyOperation
    {
        public Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }
}
