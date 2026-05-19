namespace sc.lint.roslyn.demagic.tests.packagevalidation;

public sealed record PackageValidationResult(
    bool Success,
    IReadOnlyList<string> MissingExpectedDiagnostics,
    IReadOnlyList<string> UnexpectedDiagnostics,
    IReadOnlyList<string> DiagnosticsReportedForCleanFiles);
