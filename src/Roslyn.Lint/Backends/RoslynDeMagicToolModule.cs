namespace Roslyn.Lint.Backends;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Contracts;

public sealed class RoslynDeMagicToolModule : ILintToolModule
{
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
        new ToolId("demagic"),
        "Roslyn.DeMagic",
        "Detects and reports forbidden magic string usage.",
        "Roslyn.DeMagic",
        ["lint", "view"],
        ["lint.demagic", "view.tools"]);

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
}
