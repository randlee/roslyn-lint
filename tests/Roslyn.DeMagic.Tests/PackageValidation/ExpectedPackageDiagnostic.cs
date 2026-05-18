namespace Roslyn.DeMagic.Tests.PackageValidation;

public sealed record ExpectedPackageDiagnostic(
    string File,
    string Id,
    string Severity,
    PackageValidationSampleKind SampleKind);
