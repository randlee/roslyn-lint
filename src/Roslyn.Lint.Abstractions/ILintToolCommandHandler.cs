namespace Roslyn.Lint.Abstractions;

public interface ILintToolCommandHandler<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
