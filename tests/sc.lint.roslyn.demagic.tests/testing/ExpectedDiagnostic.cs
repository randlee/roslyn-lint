namespace sc.lint.roslyn.demagic.tests.testing;

using Microsoft.CodeAnalysis;

public sealed record ExpectedDiagnostic(
    string Id,
    DiagnosticSeverity Severity,
    params string[] MessageSubstrings);
