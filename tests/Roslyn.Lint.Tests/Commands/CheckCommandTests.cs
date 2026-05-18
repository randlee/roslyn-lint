namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
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

    private sealed class StubCheckOperation(CheckResult result) : ICheckOperation
    {
        public Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }
}
