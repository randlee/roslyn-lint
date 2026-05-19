namespace Roslyn.DeMagic.Tests.PackageValidation;

public sealed record PackageValidationResult(
    bool Success,
    IReadOnlyList<string> MissingExpectedDiagnostics,
    IReadOnlyList<string> UnexpectedDiagnostics,
    IReadOnlyList<string> DiagnosticsReportedForCleanFiles);
