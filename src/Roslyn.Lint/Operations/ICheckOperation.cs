namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface ICheckOperation
{
    Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken);
}
