namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Contracts;

public interface IBackendToolDispatcher
{
    BackendToolDescriptor GetRequiredTool(ToolId toolId);

    Task<LintToolResult> DispatchAsync(LintToolRequest request, CancellationToken cancellationToken);
}
