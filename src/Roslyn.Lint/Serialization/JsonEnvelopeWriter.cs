namespace Roslyn.Lint.Serialization;

using System.Text.Json;

public sealed class JsonEnvelopeWriter : IJsonEnvelopeWriter
{
    public Task WriteAsync(TextWriter writer, object envelope, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(
            envelope,
            envelope.GetType(),
            RoslynLintJsonContext.Default.Options);

        return writer.WriteLineAsync(json);
    }
}
