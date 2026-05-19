namespace sc.lint.roslyn.demagic.tests.configuration;

using FluentAssertions;
using sc.lint.roslyn.demagic.configuration;
using Xunit;

public sealed class ConfiguredSeverityTests
{
    [Theory]
    [InlineData(ConfiguredSeverity.Hidden)]
    [InlineData(ConfiguredSeverity.Info)]
    [InlineData(ConfiguredSeverity.Warning)]
    [InlineData(ConfiguredSeverity.Error)]
    public void Enum_ContainsExpectedValues(ConfiguredSeverity severity)
    {
        Enum.IsDefined(typeof(ConfiguredSeverity), severity).Should().BeTrue();
    }
}
