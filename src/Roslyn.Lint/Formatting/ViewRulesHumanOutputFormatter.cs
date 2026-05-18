namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class ViewRulesHumanOutputFormatter : IHumanOutputFormatter<ViewResult>
{
    public async Task WriteAsync(TextWriter writer, ViewResult response, CancellationToken cancellationToken)
    {
        foreach (var rule in response.Rules ?? [])
        {
            await writer.WriteLineAsync($"{rule.Tool}/{rule.Id} - {rule.Title}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  category: {rule.Category}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  severity: {rule.DefaultSeverity}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  message: {rule.MessageFormat}".AsMemory(), cancellationToken);

            if (!string.IsNullOrWhiteSpace(rule.Description))
            {
                await writer.WriteLineAsync($"  description: {rule.Description}".AsMemory(), cancellationToken);
            }
        }
    }
}
