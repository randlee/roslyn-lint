namespace sc.lint.roslyn.abstractions;

using sc.lint.roslyn.abstractions.contracts;

/// <summary>
/// Backend seam for lint-tool workspace loading and analyzer execution.
/// </summary>
public interface ILintWorkspaceAdapter
{
    Task<LintToolResult> ExecuteLintAsync(LintToolRequest request, CancellationToken cancellationToken);
}
