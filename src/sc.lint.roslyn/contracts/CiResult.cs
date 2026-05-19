namespace sc.lint.roslyn.contracts;

public sealed record CiResult(
    string TargetPath,
    string Configuration,
    string Status,
    LintProfileResult Lint,
    IReadOnlyList<WorkflowStepResult> Steps);
