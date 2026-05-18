namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface ICiOperation
{
    Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken);
}
