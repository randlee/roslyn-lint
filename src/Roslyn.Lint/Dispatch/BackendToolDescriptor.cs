namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.CommandModel;
using Roslyn.Lint.Contracts;

public sealed record BackendToolDescriptor(
    ToolDescriptor Tool,
    BackendExecutionMode ExecutionMode,
    ILintToolCommandHandler<LintToolRequest, LintToolResult> Handler);
