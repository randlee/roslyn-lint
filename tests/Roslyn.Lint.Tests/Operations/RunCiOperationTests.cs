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
        var runner = new StubRunner(
            new DotnetCommandResult("/repo", ["test", "tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj"], 0, string.Empty, string.Empty),
            new DotnetCommandResult("/repo", ["test", "tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj"], 0, string.Empty, string.Empty));
        var operation = new RunCiOperation(lintOperation, runner);

        var result = await operation.ExecuteAsync(new CiRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        result.Lint.Profile.Should().Be("ci");
        result.Steps.Should().HaveCount(2);
        result.Steps.Should().Contain(step => step.Name == "test-demagic");
        result.Steps.Should().Contain(step => step.Name == "test-cli");
        lintOperation.Requests.Should().ContainSingle(request => request.Tool.Value == "demagic");
        runner.Invocations.Should().HaveCount(2);
        runner.Invocations[0].Should().Contain("tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj");
        runner.Invocations[1].Should().Contain("tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj");
    }

    [Fact]
    public async Task ExecuteAsync_WhenTestFails_ThrowsDotnetCommandFailedException()
    {
        var lintOperation = new StubLintOperation();
        var runner = new StubRunner(
            new DotnetCommandResult("/repo", ["test", "tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj"], 0, string.Empty, string.Empty),
            new DotnetCommandResult("/repo", ["test", "tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj"], 1, string.Empty, "boom"));
        var operation = new RunCiOperation(lintOperation, runner);

        var act = () => operation.ExecuteAsync(new CiRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        var exception = await act.Should().ThrowAsync<DotnetCommandFailedException>();
        exception.Which.StepName.Should().Be("test");
    }

    [Fact]
    public async Task ExecuteAsync_WhenDotnetIsUnavailable_PropagatesDotnetToolUnavailableException()
    {
        var operation = new RunCiOperation(new StubLintOperation(), new ThrowingRunner(new DotnetToolUnavailableException(new InvalidOperationException("missing dotnet"))));

        var act = () => operation.ExecuteAsync(new CiRequest(GetRepoRoot(), "Release"), CancellationToken.None);

        await act.Should().ThrowAsync<DotnetToolUnavailableException>();
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

    private sealed class StubRunner(params DotnetCommandResult[] results) : IDotnetCommandRunner
    {
        private readonly Queue<DotnetCommandResult> results = new(results);

        public List<IReadOnlyList<string>> Invocations { get; } = [];

        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
        {
            Invocations.Add(arguments.ToArray());
            return Task.FromResult(results.Dequeue() with { WorkingDirectory = workingDirectory, Arguments = arguments.ToArray() });
        }
    }

    private sealed class ThrowingRunner(Exception exception) : IDotnetCommandRunner
    {
        public Task<DotnetCommandResult> RunAsync(string workingDirectory, IReadOnlyList<string> arguments, CancellationToken cancellationToken)
            => Task.FromException<DotnetCommandResult>(exception);
    }
}
