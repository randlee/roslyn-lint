namespace sc.lint.roslyn.dispatch;

public interface IBackendProcessRunner
{
    Task<BackendProcessResult> RunAsync(BackendProcessRequest request, CancellationToken cancellationToken);
}

public sealed record BackendProcessRequest(
    string FileName,
    string WorkingDirectory,
    IReadOnlyList<string> Arguments);

public sealed record BackendProcessResult(
    string FileName,
    string WorkingDirectory,
    IReadOnlyList<string> Arguments,
    int ExitCode,
    string StandardOutput,
    string StandardError)
{
    public string ArgumentsDisplay => string.Join(" ", Arguments);
}
