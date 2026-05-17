namespace Roslyn.Lint.Contracts;

using System.Collections.Immutable;

public sealed record LintRequest(
    string Path,
    bool Json,
    ImmutableArray<string> IncludePatterns,
    ImmutableArray<string> ExcludePatterns,
    bool NoColor);
