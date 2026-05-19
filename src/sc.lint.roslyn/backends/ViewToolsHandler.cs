namespace sc.lint.roslyn.backends;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public sealed class ViewToolsHandler : ILintToolCommandHandler<ViewRequest, ViewResult>
{
    private readonly IReadOnlyList<ILintToolModule> toolModules;

    public ViewToolsHandler(IReadOnlyList<ILintToolModule> toolModules)
    {
        this.toolModules = toolModules;
    }

    public Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken)
    {
        var tools = toolModules
            .Select(module => new ViewToolResult(
                module.Descriptor.Id.Value,
                module.Descriptor.DisplayName,
                module.Descriptor.Description,
                module.Descriptor.PackageName,
                module.Descriptor.CommandFamilies,
                module.Descriptor.Capabilities))
            .ToArray();

        return Task.FromResult(new ViewResult("tools", tools: tools));
    }
}
