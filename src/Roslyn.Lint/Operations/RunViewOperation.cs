namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Backends;
using Roslyn.Lint.Contracts;

public sealed class RunViewOperation : IViewOperation
{
    private readonly ViewToolsHandler viewToolsHandler;
    private readonly ViewRulesHandler viewRulesHandler;

    public RunViewOperation(ViewToolsHandler viewToolsHandler, ViewRulesHandler viewRulesHandler)
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
