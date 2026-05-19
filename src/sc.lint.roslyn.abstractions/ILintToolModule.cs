namespace sc.lint.roslyn.abstractions;

public interface ILintToolModule
{
    ToolDescriptor Descriptor { get; }

    IReadOnlyList<ToolRuleDescriptor> Rules { get; }

    bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? handler);
}
