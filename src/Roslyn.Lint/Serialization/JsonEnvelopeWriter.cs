namespace Roslyn.Lint.Serialization;

using System.Text.Json;

public sealed class JsonEnvelopeWriter : IJsonEnvelopeWriter
{
    public async Task WriteAsync(TextWriter writer, object envelope, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(
            envelope,
            envelope.GetType(),
            RoslynLintJsonContext.Default.Options);

        await writer.WriteLineAsync(json.AsMemory(), cancellationToken);
    }
}
