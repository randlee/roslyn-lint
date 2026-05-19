namespace Roslyn.DeMagic.Lint;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;

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
