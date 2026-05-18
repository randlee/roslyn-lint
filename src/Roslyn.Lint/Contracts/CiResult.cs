namespace Roslyn.Lint.Contracts;

public sealed record CiResult(
    string TargetPath,
    string Configuration,
    string Status,
    LintProfileResult Lint,
    IReadOnlyList<WorkflowStepResult> Steps);
