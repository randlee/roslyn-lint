namespace Roslyn.Lint.Backends;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;

public sealed class ViewRulesHandler : ILintToolCommandHandler<ViewRequest, ViewResult>
{
    private readonly IReadOnlyList<ILintToolModule> toolModules;

    public ViewRulesHandler(IReadOnlyList<ILintToolModule> toolModules)
    {
        this.toolModules = toolModules;
    }

    public Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken)
    {
        var rules = toolModules
            .Where(module => string.IsNullOrWhiteSpace(request.ToolId)
                || string.Equals(module.Descriptor.Id.Value, request.ToolId, StringComparison.OrdinalIgnoreCase))
            .SelectMany(module => module.Rules.Select(rule => new ViewRuleResult(
                module.Descriptor.Id.Value,
                rule.Id,
                rule.Title,
                rule.Category,
                rule.DefaultSeverity,
                rule.IsEnabledByDefault,
                rule.MessageFormat,
                rule.Description)))
            .ToArray();

        return Task.FromResult(new ViewResult("rules", rules: rules));
    }
}
