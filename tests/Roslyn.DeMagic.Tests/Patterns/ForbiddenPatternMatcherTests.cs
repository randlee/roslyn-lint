namespace Roslyn.DeMagic.Tests.Patterns;

using System.Collections.Immutable;
using FluentAssertions;
using Roslyn.DeMagic.Patterns;
using Xunit;

public sealed class ForbiddenPatternMatcherTests
{
    [Fact]
    public void TryMatch_ExactPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile(["atm"], caseSensitive: false);

        var matched = matcher.TryMatch("atm", patterns, out var pattern);

        matched.Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Exact);
    }

    [Fact]
    public void TryMatch_PrefixPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile(["atm*"], caseSensitive: false);

        matcher.TryMatch("atm-core", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Prefix);
    }

    [Fact]
    public void TryMatch_SuffixPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile(["*atm"], caseSensitive: false);

        matcher.TryMatch("core-atm", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Suffix);
    }

    [Fact]
    public void TryMatch_SubstringPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile(["*atm*"], caseSensitive: false);

        matcher.TryMatch("core-atm-cli", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Substring);
    }

    [Fact]
    public void TryMatch_CaseSensitivePattern_DoesNotMatchDifferentCasing()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile(["ATM"], caseSensitive: true);

        matcher.TryMatch("atm", patterns, out _).Should().BeFalse();
    }
}
