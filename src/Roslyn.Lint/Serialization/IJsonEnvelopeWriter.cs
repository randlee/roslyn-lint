namespace Roslyn.Lint.Serialization;

public interface IJsonEnvelopeWriter
{
    Task WriteAsync(TextWriter writer, object envelope, CancellationToken cancellationToken);
}
