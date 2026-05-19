namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.backends;
using sc.lint.roslyn.abstractions.contracts;

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
