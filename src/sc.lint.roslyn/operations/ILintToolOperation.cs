namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions.contracts;

public interface ILintToolOperation
{
    Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken);
}
