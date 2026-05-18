namespace Roslyn.Lint.Tests.Operations;

using FluentAssertions;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Operations;
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
            "Roslyn.DeMagic",
            "Test tool",
            "Roslyn.DeMagic",
            ["lint", "view"],
            ["lint.demagic", "view.tools", "view.rules"]);

        public IReadOnlyList<ToolRuleDescriptor> Rules { get; } =
        [
            new(
                new ToolId("demagic"),
                "DM001",
                "Constant consolidation",
                "roslyn-lint.Organization",
                "warning",
                true,
                "Constant consolidation required",
                "DM001 enforces designated-file consolidation for shared constants."),
            new(
                new ToolId("demagic"),
                "DM002",
                "Forbidden string literal",
                "roslyn-lint.DomainBoundary",
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
