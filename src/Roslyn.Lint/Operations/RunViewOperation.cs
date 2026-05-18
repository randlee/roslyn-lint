namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Abstractions.Contracts;

public sealed class RunViewOperation : IViewOperation
{
    private readonly ILintToolCommandHandler<ViewRequest, ViewResult> viewToolsHandler;
    private readonly ILintToolCommandHandler<ViewRequest, ViewResult> viewRulesHandler;

    public RunViewOperation(
        ILintToolCommandHandler<ViewRequest, ViewResult> viewToolsHandler,
        ILintToolCommandHandler<ViewRequest, ViewResult> viewRulesHandler)
    {
        this.viewToolsHandler = viewToolsHandler;
        this.viewRulesHandler = viewRulesHandler;
    }

    public Task<ViewResult> ExecuteAsync(ViewRequest request, CancellationToken cancellationToken)
        => request.Target switch
        {
            "tools" => viewToolsHandler.ExecuteAsync(request, cancellationToken),
            "rules" => viewRulesHandler.ExecuteAsync(request, cancellationToken),
            _ => throw new InvalidOperationException($"Unsupported view target '{request.Target}'."),
        };
}
