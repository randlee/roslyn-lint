namespace Roslyn.Lint.Tests.Dispatch;

using FluentAssertions;
using Roslyn.Lint.Dispatch;
using Xunit;

public sealed class ProcessBackendRunnerTests
{
    [Fact]
    public async Task RunAsync_WithDotnetVersion_CapturesBackendOutput()
    {
        var runner = new ProcessBackendRunner();
        var request = new BackendProcessRequest(
            "dotnet",
            GetRepoRoot(),
            ["--version"]);

        var result = await runner.RunAsync(request, CancellationToken.None);

        result.ExitCode.Should().Be(0);
        result.FileName.Should().Be("dotnet");
        result.StandardOutput.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task RunAsync_WithMissingExecutable_ThrowsBackendProcessUnavailableException()
    {
        var runner = new ProcessBackendRunner();
        var request = new BackendProcessRequest(
            "definitely-not-a-real-roslyn-lint-backend",
            GetRepoRoot(),
            []);

        var act = () => runner.RunAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .Where(exception => exception.GetType().Name == "BackendProcessUnavailableException");
    }

    private static string GetRepoRoot()
        => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
}
