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

    [Fact]
    public async Task ExecuteAsync_WhenBuildFails_ThrowsDotnetCommandFailedException()
    {
        var runner = new StubRunner(new DotnetCommandResult("/repo", ["build"], 1, string.Empty, "boom"));
        var operation = new RunClippyOperation(runner);

        var act = () => operation.ExecuteAsync(new ClippyRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        var exception = await act.Should().ThrowAsync<DotnetCommandFailedException>();
        exception.Which.StepName.Should().Be("build");
    }

    [Fact]
    public async Task ExecuteAsync_WhenFormatFails_ThrowsDotnetCommandFailedException()
    {
        var runner = new StubRunner(
            new DotnetCommandResult("/repo", ["build"], 0, string.Empty, string.Empty),
            new DotnetCommandResult("/repo", ["format"], 2, string.Empty, "boom"));
        var operation = new RunClippyOperation(runner);

        var act = () => operation.ExecuteAsync(new ClippyRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        var exception = await act.Should().ThrowAsync<DotnetCommandFailedException>();
        exception.Which.StepName.Should().Be("format");
    }

    [Fact]
    public async Task ExecuteAsync_WhenDotnetIsUnavailable_PropagatesDotnetToolUnavailableException()
    {
        var operation = new RunClippyOperation(new ThrowingRunner(new DotnetToolUnavailableException(new InvalidOperationException("missing dotnet"))));

        var act = () => operation.ExecuteAsync(new ClippyRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        await act.Should().ThrowAsync<DotnetToolUnavailableException>();
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

    private sealed class ThrowingRunner(Exception exception) : IDotnetCommandRunner
    {
        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
            => Task.FromException<DotnetCommandResult>(exception);
    }
}
