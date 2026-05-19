namespace sc.lint.roslyn.dispatch;

using System.ComponentModel;
using System.Diagnostics;

internal sealed class BackendProcessUnavailableException : Exception
{
    public BackendProcessUnavailableException(string fileName, Exception innerException)
        : base($"The backend process '{fileName}' is not available.", innerException)
    {
        FileName = fileName;
    }

    public string FileName { get; }
}

public sealed class ProcessBackendRunner : IBackendProcessRunner
{
    public async Task<BackendProcessResult> RunAsync(BackendProcessRequest request, CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo(request.FileName)
        {
            WorkingDirectory = request.WorkingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var argument in request.Arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        try
        {
            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException($"Failed to start backend process '{request.FileName}'.");

            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                TryKill(process);
                throw;
            }

            return new BackendProcessResult(
                request.FileName,
                request.WorkingDirectory,
                request.Arguments.ToArray(),
                process.ExitCode,
                await outputTask,
                await errorTask);
        }
        catch (Exception exception) when (exception is Win32Exception or FileNotFoundException)
        {
            throw new BackendProcessUnavailableException(request.FileName, exception);
        }
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
            // Best effort only.
        }
    }
}
