namespace sc.lint.roslyn.demagic.tests.packagevalidation;

public sealed record PackageValidationManifest(
    string PackageId,
    string PackageVersion,
    string SampleProject,
    IReadOnlyList<ExpectedPackageDiagnostic> ExpectedDiagnostics,
    IReadOnlyList<string> ExpectedCleanFiles);
