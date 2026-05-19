namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.dispatch;

public sealed class RunLintToolOperation(IBackendToolDispatcher dispatcher) : ILintToolOperation
{
    public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return dispatcher.DispatchAsync(request, cancellationToken);
    }
}
