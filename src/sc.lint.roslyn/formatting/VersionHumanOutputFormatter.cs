namespace sc.lint.roslyn.formatting;

using sc.lint.roslyn.contracts;

public sealed class VersionHumanOutputFormatter : IHumanOutputFormatter<VersionResult>
{
    public Task WriteAsync(TextWriter writer, VersionResult response, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return writer.WriteLineAsync(response.Version.AsMemory(), cancellationToken);
    }
}
