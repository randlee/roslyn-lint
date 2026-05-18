namespace Roslyn.Lint.Abstractions;

public interface ILintToolModule
{
    ToolDescriptor Descriptor { get; }

    bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? handler);
}
