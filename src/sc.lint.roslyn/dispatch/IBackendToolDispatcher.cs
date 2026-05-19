namespace sc.lint.roslyn.dispatch;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public interface IBackendToolDispatcher
{
    BackendToolDescriptor GetRequiredTool(ToolId toolId);

    Task<LintToolResult> DispatchAsync(LintToolRequest request, CancellationToken cancellationToken);
}
