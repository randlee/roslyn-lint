namespace Roslyn.Lint.Contracts;

public sealed record ClippyResult(
    string TargetPath,
    string Configuration,
    string Status,
    IReadOnlyList<WorkflowStepResult> Steps);
