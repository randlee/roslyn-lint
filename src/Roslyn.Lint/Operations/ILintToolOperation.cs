namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Abstractions.Contracts;

public interface ILintToolOperation
{
    Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken);
}
