namespace Roslyn.Lint.Contracts;

public sealed record LintIssue(
    string Id,
    string Severity,
    string Message,
    string File,
    int Line,
    int Column);
