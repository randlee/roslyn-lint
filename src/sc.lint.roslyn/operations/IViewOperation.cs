namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions.contracts;

public interface IViewOperation
{
    Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken);
}
