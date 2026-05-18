namespace Roslyn.Lint.Tests.Dispatch;

using FluentAssertions;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Dispatch;
using Xunit;

public sealed class BackendToolDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_WithRegisteredModule_ReturnsHandlerResult()
    {
        var dispatcher = new BackendToolDispatcher(
            [new TestLintToolModule(new TestLintHandler(new LintToolResult("demagic", "/repo", "pass", 0, [])))]);

        var result = await dispatcher.DispatchAsync(
            new LintToolRequest(new ToolId("demagic"), "/repo"),
            CancellationToken.None);

        result.Tool.Should().Be("demagic");
        result.Status.Should().Be("pass");
    }

    [Fact]
    public void GetRequiredTool_WithMissingModule_Throws()
    {
        var dispatcher = new BackendToolDispatcher([]);

        var action = () => dispatcher.GetRequiredTool(new ToolId("demagic"));

        action.Should().Throw<KeyNotFoundException>();
    }

    private sealed class TestLintToolModule(ILintToolCommandHandler<LintToolRequest, LintToolResult> handler) : ILintToolModule
    {
        public ToolDescriptor Descriptor { get; } = new(
            new ToolId("demagic"),
            "Roslyn.DeMagic",
            "Test tool",
            "Roslyn.DeMagic",
            ["lint"],
            ["lint.demagic"]);

        public bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? resolvedHandler)
        {
            if (typeof(TRequest) == typeof(LintToolRequest) && typeof(TResponse) == typeof(LintToolResult))
            {
                resolvedHandler = (ILintToolCommandHandler<TRequest, TResponse>)handler;
                return true;
            }

            resolvedHandler = null;
            return false;
        }
    }

    private sealed class TestLintHandler(LintToolResult result) : ILintToolCommandHandler<LintToolRequest, LintToolResult>
    {
        public Task<LintToolResult> ExecuteAsync(LintToolRequest request, CancellationToken cancellationToken)
            => Task.FromResult(result);
    }
}
