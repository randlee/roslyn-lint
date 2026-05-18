namespace Roslyn.Lint.Formatting;

public interface IHumanOutputFormatter<in TResponse>
{
    Task WriteAsync(TextWriter writer, TResponse response, CancellationToken cancellationToken);
}
