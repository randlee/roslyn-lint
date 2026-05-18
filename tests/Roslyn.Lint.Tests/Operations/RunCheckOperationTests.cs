namespace Roslyn.Lint.Tests.Operations;

using FluentAssertions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class RunCheckOperationTests
{
    [Fact]
    public async Task ExecuteAsync_RunsBuildStep()
    {
        var runner = new StubRunner(new DotnetCommandResult("/repo", ["build"], 0, string.Empty, string.Empty));
        var operation = new RunCheckOperation(runner);

        var result = await operation.ExecuteAsync(new CheckRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        result.Status.Should().Be("pass");
        runner.Invocations.Should().ContainSingle();
        runner.Invocations[0].Should().Contain("build");
    }

    private static string GetRepoRoot()
        => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    private sealed class StubRunner(DotnetCommandResult result) : IDotnetCommandRunner
    {
        public List<IReadOnlyList<string>> Invocations { get; } = [];

        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
        {
            Invocations.Add(arguments.ToArray());
            return Task.FromResult(result with { WorkingDirectory = workingDirectory, Arguments = arguments.ToArray() });
        }
    }
}
