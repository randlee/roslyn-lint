namespace Roslyn.Lint.Contracts;

public sealed record LintToolResult(
    string Tool,
    string TargetPath,
    string Status,
    int FindingCount,
    IReadOnlyList<LintFinding> Findings);
