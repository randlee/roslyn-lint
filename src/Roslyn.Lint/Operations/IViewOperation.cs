namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface IViewOperation
{
    Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken);
}
