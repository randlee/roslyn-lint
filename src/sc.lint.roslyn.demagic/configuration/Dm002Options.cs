namespace sc.lint.roslyn.demagic.configuration;

using System.Collections.Immutable;
using sc.lint.roslyn.demagic.patterns;

public sealed record Dm002Options(
    bool Enabled,
    ConfiguredSeverity Severity,
    ImmutableArray<ForbiddenPattern> ForbiddenPatterns,
    bool CaseSensitive);
