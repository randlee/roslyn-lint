namespace Roslyn.Lint.Tests.Operations;

using FluentAssertions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class RunClippyOperationTests
{
    [Fact]
    public async Task ExecuteAsync_RunsBuildAndFormatSteps()
    {
        var runner = new StubRunner(
            new DotnetCommandResult("/repo", ["build"], 0, string.Empty, string.Empty),
            new DotnetCommandResult("/repo", ["format"], 0, string.Empty, string.Empty));
        var operation = new RunClippyOperation(runner);

        var result = await operation.ExecuteAsync(new ClippyRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        result.Steps.Should().HaveCount(2);
        runner.Invocations.Should().HaveCount(2);
    }

    private static string GetRepoRoot()
        => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    private sealed class StubRunner(params DotnetCommandResult[] results) : IDotnetCommandRunner
    {
        private readonly Queue<DotnetCommandResult> results = new(results);

        public List<IReadOnlyList<string>> Invocations { get; } = [];

        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
        {
            Invocations.Add(arguments.ToArray());
            return Task.FromResult(this.results.Dequeue() with { WorkingDirectory = workingDirectory, Arguments = arguments.ToArray() });
        }
    }
}
