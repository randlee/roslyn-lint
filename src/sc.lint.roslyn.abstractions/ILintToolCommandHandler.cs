namespace sc.lint.roslyn.abstractions;

public interface ILintToolCommandHandler<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
