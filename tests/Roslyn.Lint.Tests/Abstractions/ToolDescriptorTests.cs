namespace Roslyn.Lint.Tests.Abstractions;

using FluentAssertions;
using Roslyn.Lint.Abstractions;
using Xunit;

public sealed class ToolDescriptorTests
{
    [Fact]
    public void ToolDescriptor_RetainsExplicitMetadata()
    {
        var descriptor = new ToolDescriptor(
            new ToolId("demagic"),
            "Roslyn.DeMagic",
            "Detects forbidden magic strings.",
            "Roslyn.DeMagic",
            ["lint", "view"],
            ["lint.demagic", "view.tools"]);

        descriptor.Id.Value.Should().Be("demagic");
        descriptor.DisplayName.Should().Be("Roslyn.DeMagic");
        descriptor.CommandFamilies.Should().ContainInOrder("lint", "view");
        descriptor.Capabilities.Should().Contain("lint.demagic");
    }
}
