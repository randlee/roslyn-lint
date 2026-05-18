namespace Roslyn.DeMagic.Tests.PackageValidation;

public sealed record PackageValidationManifest(
    string PackageId,
    string PackageVersion,
    string SampleProject,
    IReadOnlyList<ExpectedPackageDiagnostic> ExpectedDiagnostics,
    IReadOnlyList<string> ExpectedCleanFiles);
