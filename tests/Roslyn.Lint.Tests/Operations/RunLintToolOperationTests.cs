namespace Roslyn.Lint.Tests.Operations;

using FluentAssertions;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Dispatch;
using Roslyn.Lint.Operations;
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
