namespace sc.lint.roslyn.demagic.lint;

using sc.lint.roslyn.demagic;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public sealed class RoslynDeMagicToolModule : ILintToolModule
{
    private static readonly ToolId ToolId = ScLintRoslynConstants.Tools.DeMagicId;
    private static readonly IReadOnlyList<ToolRuleDescriptor> RulesMetadata =
    [
        CreateRuleDescriptor(
            DeMagicConstants.Dm001Id,
            DeMagicConstants.Dm001Title,
            DeMagicConstants.Dm001Category,
            "warning",
            true,
            DeMagicConstants.Dm001MessageFormat,
            DeMagicConstants.Dm001Description),
        CreateRuleDescriptor(
            DeMagicConstants.Dm002Id,
            DeMagicConstants.Dm002Title,
            DeMagicConstants.Dm002Category,
            "error",
            true,
            DeMagicConstants.Dm002MessageFormat,
            DeMagicConstants.Dm002Description),
    ];

    private readonly ILintToolCommandHandler<LintToolRequest, LintToolResult> lintHandler;

    public RoslynDeMagicToolModule()
        : this(new RoslynDeMagicLintHandler())
    {
    }

    public RoslynDeMagicToolModule(ILintToolCommandHandler<LintToolRequest, LintToolResult> lintHandler)
    {
        this.lintHandler = lintHandler;
    }

    public ToolDescriptor Descriptor { get; } = new(
        ToolId,
        ScLintRoslynConstants.Tools.DeMagicModuleName,
        "Detects and reports forbidden magic string usage.",
        ScLintRoslynConstants.Tools.DeMagicModuleName,
        ["lint", "view"],
        [ScLintRoslynConstants.Commands.LintDemagic, ScLintRoslynConstants.Commands.ViewTools, ScLintRoslynConstants.Commands.ViewRules]);

    public IReadOnlyList<ToolRuleDescriptor> Rules { get; } = RulesMetadata;

    public bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? handler)
    {
        if (typeof(TRequest) == typeof(LintToolRequest) && typeof(TResponse) == typeof(LintToolResult))
        {
            handler = (ILintToolCommandHandler<TRequest, TResponse>)lintHandler;
            return true;
        }

        handler = null;
        return false;
    }

    private static ToolRuleDescriptor CreateRuleDescriptor(
        string id,
        string title,
        string category,
        string defaultSeverity,
        bool isEnabledByDefault,
        string messageFormat,
        string description)
        => new(
            ToolId,
            id,
            title,
            category,
            defaultSeverity,
            isEnabledByDefault,
            messageFormat,
            description);
}
