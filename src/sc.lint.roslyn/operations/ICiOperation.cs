namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.contracts;

public interface ICiOperation
{
    Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken);
}
