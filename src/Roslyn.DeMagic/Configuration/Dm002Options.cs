namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Roslyn.DeMagic.Patterns;

public sealed record Dm002Options(
    bool Enabled,
    ConfiguredSeverity Severity,
    ImmutableArray<ForbiddenPattern> ForbiddenPatterns,
    bool CaseSensitive);
