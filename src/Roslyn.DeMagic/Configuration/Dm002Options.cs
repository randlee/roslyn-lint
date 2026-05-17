namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;

public sealed record Dm002Options(
    bool Enabled,
    ConfiguredSeverity Severity,
    ImmutableArray<string> ForbiddenPatterns,
    bool CaseSensitive);
