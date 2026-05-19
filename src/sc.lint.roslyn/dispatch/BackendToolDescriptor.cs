namespace sc.lint.roslyn.dispatch;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.commandmodel;

public sealed record BackendToolDescriptor(
    ToolDescriptor Tool,
    BackendExecutionMode ExecutionMode,
    ILintToolCommandHandler<LintToolRequest, LintToolResult> Handler);
