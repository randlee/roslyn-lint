namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Abstractions.Contracts;

public interface IViewOperation
{
    Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken);
}
