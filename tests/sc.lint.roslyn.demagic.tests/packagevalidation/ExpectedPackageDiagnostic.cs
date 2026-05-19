namespace sc.lint.roslyn.demagic.tests.packagevalidation;

public sealed record ExpectedPackageDiagnostic(
    string File,
    string Id,
    string Severity,
    PackageValidationSampleKind SampleKind);
