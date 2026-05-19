namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class ClippyHumanOutputFormatter : IHumanOutputFormatter<ClippyResult>
{
    public async Task WriteAsync(TextWriter writer, ClippyResult response, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync($"clippy: {response.Status}".AsMemory(), cancellationToken);

        foreach (var step in response.Steps)
        {
            await writer.WriteLineAsync(
                $"{step.Name}: {step.Status} (exit {step.ExitCode})".AsMemory(),
                cancellationToken);
        }
    }
}
