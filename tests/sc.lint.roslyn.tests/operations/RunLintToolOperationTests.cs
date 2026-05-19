namespace sc.lint.roslyn.tests.operations;

using FluentAssertions;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.dispatch;
using sc.lint.roslyn.operations;
using Xunit;

public sealed class RunLintToolOperationTests
{
    [Fact]
    public async Task ExecuteAsync_DelegatesToDispatcher()
    {
        var dispatcher = new TestBackendToolDispatcher();
        var operation = new RunLintToolOperation(dispatcher);
        var request = new LintToolRequest(new ToolId("demagic"), "/repo");

        var result = await operation.ExecuteAsync(request, CancellationToken.None);

        dispatcher.CapturedRequest.Should().BeEquivalentTo(request);
        result.Tool.Should().Be("demagic");
    }

    private sealed class TestBackendToolDispatcher : IBackendToolDispatcher
    {
        public LintToolRequest? CapturedRequest { get; private set; }

        public BackendToolDescriptor GetRequiredTool(ToolId toolId)
            => throw new NotSupportedException();

        public Task<LintToolResult> DispatchAsync(LintToolRequest request, CancellationToken cancellationToken)
        {
            CapturedRequest = request;
            return Task.FromResult(new LintToolResult("demagic", request.TargetPath, "pass", 0, []));
        }
    }
}
