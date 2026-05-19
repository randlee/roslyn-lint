namespace sc.lint.roslyn.contracts;

using sc.lint.roslyn.abstractions.contracts;

public sealed record LintProfileResult(
    string Profile,
    string TargetPath,
    string Status,
    int FindingCount,
    IReadOnlyList<string> Members,
    IReadOnlyList<LintFinding> Findings);
