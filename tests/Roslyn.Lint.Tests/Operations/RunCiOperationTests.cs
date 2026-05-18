namespace Roslyn.Lint.Tests.Operations;

using FluentAssertions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class RunCiOperationTests
{
    [Fact]
    public async Task ExecuteAsync_RunsLintProfileAndTests()
    {
        var lintOperation = new StubLintOperation();
        var runner = new StubRunner(new DotnetCommandResult("/repo", ["test"], 0, string.Empty, string.Empty));
        var operation = new RunCiOperation(lintOperation, runner);

        var result = await operation.ExecuteAsync(new CiRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        result.Lint.Profile.Should().Be("ci");
        result.Steps.Should().ContainSingle(step => step.Name == "test");
        lintOperation.Requests.Should().ContainSingle(request => request.Tool.Value == "demagic");
    }

    private static string GetRepoRoot()
        => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    private sealed class StubLintOperation : ILintToolOperation
    {
        public List<LintToolRequest> Requests { get; } = [];

        public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            return Task.FromResult(new LintToolResult("demagic", request.TargetPath, "pass", 0, []));
        }
    }

    private sealed class StubRunner(DotnetCommandResult result) : IDotnetCommandRunner
    {
        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
            => Task.FromResult(result with { WorkingDirectory = workingDirectory, Arguments = arguments.ToArray() });
    }
}
