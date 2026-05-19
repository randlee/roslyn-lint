namespace Roslyn.Lint.Abstractions;

public interface ILintToolModule
{
    ToolDescriptor Descriptor { get; }

    IReadOnlyList<ToolRuleDescriptor> Rules { get; }

    bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? handler);
}
