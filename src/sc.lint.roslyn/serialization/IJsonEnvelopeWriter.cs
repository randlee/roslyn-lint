namespace sc.lint.roslyn.serialization;

public interface IJsonEnvelopeWriter
{
    Task WriteAsync(TextWriter writer, object envelope, CancellationToken cancellationToken);
}
