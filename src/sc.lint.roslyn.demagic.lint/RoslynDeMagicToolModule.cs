namespace sc.lint.roslyn.demagic.lint;

using sc.lint.roslyn.demagic.diagnostics;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public sealed class RoslynDeMagicToolModule : ILintToolModule
{
    private static readonly ToolId ToolId = new("demagic");
    private static readonly IReadOnlyList<ToolRuleDescriptor> RulesMetadata =
    [
        CreateRuleDescriptor(
            DeMagicDiagnosticDescriptors.Dm001Id,
            DeMagicDiagnosticDescriptors.Dm001Title,
            DeMagicDiagnosticDescriptors.Dm001Category,
            "warning",
            true,
            DeMagicDiagnosticDescriptors.Dm001MessageFormat,
            DeMagicDiagnosticDescriptors.Dm001Description),
        CreateRuleDescriptor(
            DeMagicDiagnosticDescriptors.Dm002Id,
            DeMagicDiagnosticDescriptors.Dm002Title,
            DeMagicDiagnosticDescriptors.Dm002Category,
            "error",
            true,
            DeMagicDiagnosticDescriptors.Dm002MessageFormat,
            DeMagicDiagnosticDescriptors.Dm002Description),
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
        "sc.lint.roslyn.demagic",
        "Detects and reports forbidden magic string usage.",
        "sc.lint.roslyn.demagic",
        ["lint", "view"],
        ["lint.demagic", "view.tools", "view.rules"]);

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
