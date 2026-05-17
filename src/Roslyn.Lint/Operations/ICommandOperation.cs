namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Contracts;

public interface ICommandOperation<TRequest, TResponse>
{
    Task<CliEnvelope<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
