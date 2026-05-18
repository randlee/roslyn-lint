namespace Roslyn.DeMagic.Lint;

using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Analyzers;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;

public sealed class RoslynDeMagicToolModule : ILintToolModule
{
    private static readonly ToolId ToolId = new("demagic");
    private static readonly IReadOnlyList<ToolRuleDescriptor> RulesMetadata =
    [
        CreateRuleDescriptor(new DM001ConstantConsolidationAnalyzer().SupportedDiagnostics[0]),
        CreateRuleDescriptor(new DM002ForbiddenStringLiteralAnalyzer().SupportedDiagnostics[0]),
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
        "Roslyn.DeMagic",
        "Detects and reports forbidden magic string usage.",
        "Roslyn.DeMagic",
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

    private static ToolRuleDescriptor CreateRuleDescriptor(DiagnosticDescriptor descriptor)
        => new(
            ToolId,
            descriptor.Id,
            descriptor.Title.ToString(),
            descriptor.Category,
            descriptor.DefaultSeverity.ToString().ToLowerInvariant(),
            descriptor.IsEnabledByDefault,
            descriptor.MessageFormat.ToString(),
            descriptor.Description.ToString());
}
