namespace Roslyn.Lint.Contracts;

public sealed record LintFinding(
    string DiagnosticId,
    string Severity,
    string Category,
    string Message,
    string FilePath,
    int Line,
    int Column);
