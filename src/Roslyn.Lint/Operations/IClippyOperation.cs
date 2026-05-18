namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface IClippyOperation
{
    Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken);
}
