namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Contracts;

public sealed class BackendToolDispatcher : IBackendToolDispatcher
{
    private readonly IReadOnlyDictionary<string, BackendToolDescriptor> tools;

    public BackendToolDispatcher(IReadOnlyList<ILintToolModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        tools = modules
            .Select(CreateDescriptor)
            .ToDictionary(
                descriptor => descriptor.Tool.Id.Value,
                descriptor => descriptor,
                StringComparer.OrdinalIgnoreCase);
    }

    public BackendToolDescriptor GetRequiredTool(ToolId toolId)
    {
        if (tools.TryGetValue(toolId.Value, out var descriptor))
            return descriptor;

        throw new KeyNotFoundException($"No backend tool registered for '{toolId.Value}'.");
    }

    public Task<LintToolResult> DispatchAsync(LintToolRequest request, CancellationToken cancellationToken)
        => GetRequiredTool(request.Tool).Handler.ExecuteAsync(request, cancellationToken);

    private static BackendToolDescriptor CreateDescriptor(ILintToolModule module)
    {
        if (!module.TryResolveCommandHandler<LintToolRequest, LintToolResult>(out var handler) || handler is null)
        {
            throw new InvalidOperationException(
                $"Tool module '{module.Descriptor.Id.Value}' does not provide a lint handler.");
        }

        return new BackendToolDescriptor(module.Descriptor, CommandModel.BackendExecutionMode.InProcess, handler);
    }
}
