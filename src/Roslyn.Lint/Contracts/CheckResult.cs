namespace Roslyn.Lint.Contracts;

public sealed record CheckResult(
    string TargetPath,
    string Configuration,
    string Status,
    IReadOnlyList<WorkflowStepResult> Steps);
