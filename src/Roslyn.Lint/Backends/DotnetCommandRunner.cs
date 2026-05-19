namespace Roslyn.Lint.Backends;

using System.Diagnostics;
using System.Text;

public interface IDotnetCommandRunner
{
    Task<DotnetCommandResult> RunAsync(
        string workingDirectory,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken);
}

public sealed record DotnetCommandResult(
    string WorkingDirectory,
    IReadOnlyList<string> Arguments,
    int ExitCode,
    string StandardOutput,
    string StandardError)
{
    public bool Succeeded => ExitCode == 0;

    public string ArgumentsDisplay => string.Join(" ", Arguments);
}

internal sealed class DotnetToolUnavailableException : Exception
{
    public DotnetToolUnavailableException(Exception innerException)
        : base("The dotnet CLI was not available.", innerException)
    {
    }
}

internal sealed class DotnetCommandFailedException : Exception
{
    public DotnetCommandFailedException(string stepName, DotnetCommandResult result)
        : base($"dotnet step '{stepName}' failed with exit code {result.ExitCode}.")
    {
        StepName = stepName;
        Result = result;
    }

    public string StepName { get; }

    public DotnetCommandResult Result { get; }
}

internal sealed class CiLintGateFailedException : Exception
{
    public CiLintGateFailedException(Contracts.LintProfileResult result)
        : base($"lint profile '{result.Profile}' reported findings.")
    {
        Result = result;
    }

    public Contracts.LintProfileResult Result { get; }
}

internal sealed class DotnetCommandRunner : IDotnetCommandRunner
{
    public async Task<DotnetCommandResult> RunAsync(
        string workingDirectory,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        try
        {
            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Failed to start the dotnet process.");

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

            var standardOutput = await outputTask;
            var standardError = await errorTask;

            return new DotnetCommandResult(
                workingDirectory,
                arguments.ToArray(),
                process.ExitCode,
                standardOutput,
                standardError);
        }
        catch (Exception exception) when (exception is System.ComponentModel.Win32Exception or FileNotFoundException)
        {
            throw new DotnetToolUnavailableException(exception);
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
