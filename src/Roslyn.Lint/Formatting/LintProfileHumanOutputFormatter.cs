namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class LintProfileHumanOutputFormatter : IHumanOutputFormatter<LintProfileResult>
{
    public async Task WriteAsync(TextWriter writer, LintProfileResult response, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync(
            $"{response.Profile}: {response.Status} ({response.FindingCount} findings)".AsMemory(),
            cancellationToken);

        await writer.WriteLineAsync(
            $"members: {string.Join(", ", response.Members)}".AsMemory(),
            cancellationToken);

        foreach (var finding in response.Findings)
        {
            await writer.WriteLineAsync(
                $"{finding.FilePath}({finding.Line},{finding.Column}): {finding.Severity} {finding.DiagnosticId} {finding.Message}".AsMemory(),
                cancellationToken);
        }
    }
}
