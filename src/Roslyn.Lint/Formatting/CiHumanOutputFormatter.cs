namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class CiHumanOutputFormatter : IHumanOutputFormatter<CiResult>
{
    public async Task WriteAsync(TextWriter writer, CiResult response, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync($"ci: {response.Status}".AsMemory(), cancellationToken);
        await writer.WriteLineAsync(
            $"lint profile {response.Lint.Profile}: {response.Lint.Status} ({response.Lint.FindingCount} findings)".AsMemory(),
            cancellationToken);

        foreach (var step in response.Steps)
        {
            await writer.WriteLineAsync(
                $"{step.Name}: {step.Status} (exit {step.ExitCode})".AsMemory(),
                cancellationToken);
        }
    }
}
