namespace Roslyn.Lint.Formatting;

using Roslyn.Lint.Contracts;

public sealed class ViewToolsHumanOutputFormatter : IHumanOutputFormatter<ViewResult>
{
    public async Task WriteAsync(TextWriter writer, ViewResult response, CancellationToken cancellationToken)
    {
        foreach (var tool in response.Tools ?? [])
        {
            await writer.WriteLineAsync($"{tool.Id} - {tool.DisplayName}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  package: {tool.PackageName}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  families: {string.Join(", ", tool.CommandFamilies)}".AsMemory(), cancellationToken);
            await writer.WriteLineAsync($"  capabilities: {string.Join(", ", tool.Capabilities)}".AsMemory(), cancellationToken);
        }
    }
}
