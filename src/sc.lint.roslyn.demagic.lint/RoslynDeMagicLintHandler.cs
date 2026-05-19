namespace sc.lint.roslyn.demagic.lint;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public sealed class RoslynDeMagicLintHandler : ILintToolCommandHandler<LintToolRequest, LintToolResult>
{
    private readonly ILintWorkspaceAdapter workspaceAdapter;

    public RoslynDeMagicLintHandler()
        : this(new DeMagicWorkspaceAdapter())
    {
    }

    public RoslynDeMagicLintHandler(ILintWorkspaceAdapter workspaceAdapter)
    {
        this.workspaceAdapter = workspaceAdapter;
    }

    public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
        => workspaceAdapter.ExecuteLintAsync(request, cancellationToken);
}
