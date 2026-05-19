namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class CheckHumanOutputFormatter : IHumanOutputFormatter<CheckResult>
{
    public async Task WriteAsync(TextWriter writer, CheckResult response, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync($"check: {response.Status}".AsMemory(), cancellationToken);

        foreach (var step in response.Steps)
        {
            await writer.WriteLineAsync(
                $"{step.Name}: {step.Status} (exit {step.ExitCode})".AsMemory(),
                cancellationToken);
        }
    }
}
