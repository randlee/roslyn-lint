namespace sc.lint.roslyn.tests.abstractions;

using FluentAssertions;
using sc.lint.roslyn.abstractions;
using Xunit;

public sealed class ToolDescriptorTests
{
    [Fact]
    public void ToolDescriptor_RetainsExplicitMetadata()
    {
        var descriptor = new ToolDescriptor(
            new ToolId("demagic"),
            "sc.lint.roslyn.demagic",
            "Detects forbidden magic strings.",
            "sc.lint.roslyn.demagic",
            ["lint", "view"],
            ["lint.demagic", "view.tools"]);

        descriptor.Id.Value.Should().Be("demagic");
        descriptor.DisplayName.Should().Be("sc.lint.roslyn.demagic");
        descriptor.CommandFamilies.Should().ContainInOrder("lint", "view");
        descriptor.Capabilities.Should().Contain("lint.demagic");
    }
}
