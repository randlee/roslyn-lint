namespace sc.lint.roslyn.contracts;

public sealed record ClippyResult(
    string TargetPath,
    string Configuration,
    string Status,
    IReadOnlyList<WorkflowStepResult> Steps);
