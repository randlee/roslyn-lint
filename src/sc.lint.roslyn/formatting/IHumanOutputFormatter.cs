namespace sc.lint.roslyn.formatting;

public interface IHumanOutputFormatter<in TResponse>
{
    Task WriteAsync(TextWriter writer, TResponse response, CancellationToken cancellationToken);
}
