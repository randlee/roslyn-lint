namespace sc.lint.roslyn.tests.dispatch;

using System.Diagnostics;
using FluentAssertions;
using sc.lint.roslyn.dispatch;
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
            "definitely-not-a-real-sc-lint-roslyn-backend",
            GetRepoRoot(),
            []);

        var act = () => runner.RunAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<BackendProcessUnavailableException>();
    }

    [Fact]
    public async Task RunAsync_WhenCancelled_KillsBackendProcess()
    {
        var runner = new ProcessBackendRunner();
        var pidFile = Path.GetTempFileName();
        File.Delete(pidFile);
        using var cancellationSource = new CancellationTokenSource();

        try
        {
            var request = CreateLongRunningRequest(pidFile);

            var runTask = runner.RunAsync(request, cancellationSource.Token);
            await WaitForFileAsync(pidFile, TimeSpan.FromSeconds(5));

            cancellationSource.Cancel();

            await FluentActions.Invoking(async () => await runTask)
                .Should().ThrowAsync<OperationCanceledException>();

            var processId = int.Parse(await File.ReadAllTextAsync(pidFile));
            await WaitForProcessExitAsync(processId, TimeSpan.FromSeconds(5));
        }
        finally
        {
            File.Delete(pidFile);
        }
    }

    private static string GetRepoRoot()
        => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));

    private static BackendProcessRequest CreateLongRunningRequest(string pidFile)
    {
        if (OperatingSystem.IsWindows())
        {
            var escapedPath = pidFile.Replace("'", "''", StringComparison.Ordinal);
            return new BackendProcessRequest(
                "powershell",
                GetRepoRoot(),
                ["-NoProfile", "-Command", $"$PID | Out-File -Encoding ascii -NoNewline '{escapedPath}'; Start-Sleep -Seconds 30"]);
        }

        var shellPath = pidFile.Replace("'", "'\"'\"'", StringComparison.Ordinal);
        return new BackendProcessRequest(
            "/bin/sh",
            GetRepoRoot(),
            ["-c", $"echo $$ > '{shellPath}'; sleep 30"]);
    }

    private static async Task WaitForFileAsync(string path, TimeSpan timeout)
    {
        var startedAt = DateTime.UtcNow;
        while (!File.Exists(path))
        {
            if (DateTime.UtcNow - startedAt > timeout)
                throw new TimeoutException($"Timed out waiting for file '{path}'.");

            await Task.Delay(50);
        }
    }

    private static async Task WaitForProcessExitAsync(int processId, TimeSpan timeout)
    {
        var startedAt = DateTime.UtcNow;
        while (true)
        {
            try
            {
                using var process = Process.GetProcessById(processId);
                process.Refresh();
                if (process.HasExited)
                    return;
            }
            catch (ArgumentException)
            {
                return;
            }

            if (DateTime.UtcNow - startedAt > timeout)
                throw new TimeoutException($"Timed out waiting for process '{processId}' to exit.");

            await Task.Delay(50);
        }
    }
}
