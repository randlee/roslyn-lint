namespace sc.lint.roslyn.tests.operations;

using FluentAssertions;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.backends;
using sc.lint.roslyn.operations;
using Xunit;

public sealed class RunViewOperationTests
{
    [Fact]
    public async Task ExecuteAsync_WithToolsTarget_ReturnsToolMetadata()
    {
        var operation = CreateOperation();

        var result = await operation.ExecuteAsync(new ViewRequest("tools"), CancellationToken.None);

        result.Target.Should().Be("tools");
        result.Tools.Should().ContainSingle(tool => tool.Id == "demagic");
        result.Rules.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_WithRulesTarget_ReturnsRuleMetadata()
    {
        var operation = CreateOperation();

        var result = await operation.ExecuteAsync(new ViewRequest("rules"), CancellationToken.None);

        result.Target.Should().Be("rules");
        result.Rules.Should().NotBeNull();
        result.Rules!.Select(rule => rule.Id).Should().Contain(["DM001", "DM002"]);
        result.Tools.Should().BeNull();
    }

    private static RunViewOperation CreateOperation()
    {
        var modules = new ILintToolModule[]
        {
            new TestModule(),
        };

        return new RunViewOperation(
            new ViewToolsHandler(modules),
            new ViewRulesHandler(modules));
    }

    private sealed class TestModule : ILintToolModule
    {
        public ToolDescriptor Descriptor { get; } = new(
            new ToolId("demagic"),
            "sc.lint.roslyn.demagic",
            "Test tool",
            "sc.lint.roslyn.demagic",
            ["lint", "view"],
            ["lint.demagic", "view.tools", "view.rules"]);

        public IReadOnlyList<ToolRuleDescriptor> Rules { get; } =
        [
            new(
                new ToolId("demagic"),
                "DM001",
                "Constant consolidation",
                "sc.lint.roslyn.organization",
                "warning",
                true,
                "Constant consolidation required",
                "DM001 enforces designated-file consolidation for shared constants."),
            new(
                new ToolId("demagic"),
                "DM002",
                "Forbidden string literal",
                "sc.lint.roslyn.domainboundary",
                "error",
                true,
                "Forbidden string literal",
                "DM002 blocks configured domain-specific string literals."),
        ];

        public bool TryResolveCommandHandler<TRequest, TResponse>(out ILintToolCommandHandler<TRequest, TResponse>? handler)
        {
            handler = null;
            return false;
        }
    }
}
