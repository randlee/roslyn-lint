namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Abstractions.Contracts;

public sealed class LintToolHumanOutputFormatter : IHumanOutputFormatter<LintToolResult>
{
    public async Task WriteAsync(TextWriter writer, LintToolResult response, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync(
            $"{response.Tool}: {response.Status} ({response.FindingCount} findings)".AsMemory(),
            cancellationToken);

        foreach (var finding in response.Findings)
        {
            await writer.WriteLineAsync(
                $"{finding.FilePath}({finding.Line},{finding.Column}): {finding.Severity} {finding.DiagnosticId} {finding.Message}".AsMemory(),
                cancellationToken);
        }
    }
}
