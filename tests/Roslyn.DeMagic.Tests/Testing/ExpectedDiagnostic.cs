namespace Roslyn.DeMagic.Tests.Testing;

using Microsoft.CodeAnalysis;

public sealed record ExpectedDiagnostic(
    string Id,
    DiagnosticSeverity Severity,
    params string[] MessageSubstrings);
