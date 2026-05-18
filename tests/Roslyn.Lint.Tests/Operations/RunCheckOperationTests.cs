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

    [Fact]
    public async Task ExecuteAsync_WhenBuildFails_ThrowsDotnetCommandFailedException()
    {
        var runner = new StubRunner(new DotnetCommandResult("/repo", ["build"], 1, string.Empty, "boom"));
        var operation = new RunCheckOperation(runner);

        var act = () => operation.ExecuteAsync(new CheckRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        var exception = await act.Should().ThrowAsync<DotnetCommandFailedException>();
        exception.Which.StepName.Should().Be("build");
    }

    [Fact]
    public async Task ExecuteAsync_WhenDotnetIsUnavailable_PropagatesDotnetToolUnavailableException()
    {
        var operation = new RunCheckOperation(new ThrowingRunner(new DotnetToolUnavailableException(new InvalidOperationException("missing dotnet"))));

        var act = () => operation.ExecuteAsync(new CheckRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        await act.Should().ThrowAsync<DotnetToolUnavailableException>();
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

    private sealed class ThrowingRunner(Exception exception) : IDotnetCommandRunner
    {
        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
            => Task.FromException<DotnetCommandResult>(exception);
    }
}
