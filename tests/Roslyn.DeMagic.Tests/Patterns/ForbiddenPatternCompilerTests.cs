namespace Roslyn.DeMagic.Tests.Patterns;

using FluentAssertions;
using Roslyn.DeMagic.Patterns;
using Xunit;

public sealed class ForbiddenPatternCompilerTests
{
    [Theory]
    [InlineData("atm", ForbiddenPatternKind.Exact, "atm")]
    [InlineData("atm*", ForbiddenPatternKind.Prefix, "atm")]
    [InlineData("*atm", ForbiddenPatternKind.Suffix, "atm")]
    [InlineData("*atm*", ForbiddenPatternKind.Substring, "atm")]
    public void Compile_AssignsExpectedPatternShape(
        string rawPattern,
        ForbiddenPatternKind expectedKind,
        string expectedMatchValue)
    {
        var matcher = new ForbiddenPatternMatcher();

        var compiled = matcher.Compile(rawPattern, caseSensitive: false);

        compiled.Kind.Should().Be(expectedKind);
        compiled.MatchValue.Should().Be(expectedMatchValue);
    }
}
