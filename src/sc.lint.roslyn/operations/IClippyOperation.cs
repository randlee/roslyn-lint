namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.contracts;

public interface IClippyOperation
{
    Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken);
}
