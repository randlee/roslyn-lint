namespace Roslyn.Lint.Contracts;

using Roslyn.Lint.Abstractions.Contracts;

public sealed record LintProfileResult(
    string Profile,
    string TargetPath,
    string Status,
    int FindingCount,
    IReadOnlyList<string> Members,
    IReadOnlyList<LintFinding> Findings);
