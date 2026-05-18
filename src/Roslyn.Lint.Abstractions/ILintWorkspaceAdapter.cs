namespace Roslyn.Lint.Abstractions;

using Roslyn.Lint.Abstractions.Contracts;

/// <summary>
/// Backend seam for lint-tool workspace loading and analyzer execution.
/// </summary>
public interface ILintWorkspaceAdapter
{
    Task<LintToolResult> ExecuteLintAsync(LintToolRequest request, CancellationToken cancellationToken);
}
