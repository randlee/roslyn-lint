namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Dispatch;

public sealed class RunLintToolOperation(IBackendToolDispatcher dispatcher) : ILintToolOperation
{
    public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return dispatcher.DispatchAsync(request, cancellationToken);
    }
}
