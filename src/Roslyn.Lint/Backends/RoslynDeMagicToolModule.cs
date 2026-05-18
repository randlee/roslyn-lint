namespace Roslyn.Lint.Backends;

using Roslyn.Lint.Abstractions;

public sealed class RoslynDeMagicToolModule : ILintToolModule
{
    public ToolDescriptor Descriptor { get; } = new(
        new ToolId("demagic"),
        "Roslyn.DeMagic",
        "Detects and reports forbidden magic string usage.",
        "Roslyn.DeMagic",
        ["lint", "view"],
        ["lint.demagic", "view.tools"]);
}
