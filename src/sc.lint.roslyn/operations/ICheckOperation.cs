namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.contracts;

public interface ICheckOperation
{
    Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken);
}
