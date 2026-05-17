namespace Roslyn.DeMagic.Tests.Configuration;

using FluentAssertions;
using Roslyn.DeMagic.Configuration;
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
