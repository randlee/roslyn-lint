namespace Roslyn.Lint.Contracts;

public sealed record WorkflowStepResult(
    string Name,
    string Tool,
    string Arguments,
    int ExitCode,
    string Status);
