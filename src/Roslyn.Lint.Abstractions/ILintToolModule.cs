namespace Roslyn.Lint.Abstractions;

public interface ILintToolModule
{
    ToolDescriptor Descriptor { get; }
}
