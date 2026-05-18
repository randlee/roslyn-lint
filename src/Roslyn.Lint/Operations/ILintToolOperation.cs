namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface ILintToolOperation
{
    Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken);
}
