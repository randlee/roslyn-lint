namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.CommandModel;

public sealed record BackendToolDescriptor(
    ToolDescriptor Tool,
    BackendExecutionMode ExecutionMode,
    ILintToolCommandHandler<LintToolRequest, LintToolResult> Handler);
