namespace sc.lint.roslyn.abstractions.contracts;

public sealed record LintFinding(
    string DiagnosticId,
    string Severity,
    string Category,
    string Message,
    string FilePath,
    int Line,
    int Column);
