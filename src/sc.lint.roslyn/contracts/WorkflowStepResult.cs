namespace sc.lint.roslyn.contracts;

public sealed record WorkflowStepResult(
    string Name,
    string Tool,
    string Arguments,
    int ExitCode,
    string Status);
