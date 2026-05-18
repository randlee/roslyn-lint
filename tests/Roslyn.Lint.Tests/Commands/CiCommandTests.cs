namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
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

    private sealed class StubCiOperation(CiResult result) : ICiOperation
    {
        public Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result with { TargetPath = request.TargetPath, Configuration = request.Configuration });
    }
}
