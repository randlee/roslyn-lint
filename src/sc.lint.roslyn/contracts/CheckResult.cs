namespace sc.lint.roslyn.contracts;

public sealed record CheckResult(
    string TargetPath,
    string Configuration,
    string Status,
    IReadOnlyList<WorkflowStepResult> Steps);
