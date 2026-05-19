namespace sc.lint.roslyn.demagic.tests.patterns;

using System.Collections.Immutable;
using FluentAssertions;
using sc.lint.roslyn.demagic.patterns;
using Xunit;

public sealed class ForbiddenPatternMatcherTests
{
    [Fact]
    public void TryMatch_ExactPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("atm")], caseSensitive: false);

        var matched = matcher.TryMatch("atm", patterns, out var pattern);

        matched.Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Exact);
    }

    [Fact]
    public void TryMatch_PrefixPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("atm*")], caseSensitive: false);

        matcher.TryMatch("atm-core", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Prefix);
    }

    [Fact]
    public void TryMatch_SuffixPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("*atm")], caseSensitive: false);

        matcher.TryMatch("core-atm", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Suffix);
    }

    [Fact]
    public void TryMatch_SubstringPattern_MatchesCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("*atm*")], caseSensitive: false);

        matcher.TryMatch("core-atm-cli", patterns, out var pattern).Should().BeTrue();
        pattern.Kind.Should().Be(ForbiddenPatternKind.Substring);
    }

    [Fact]
    public void TryMatch_CaseSensitivePattern_DoesNotMatchDifferentCasing()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("ATM")], caseSensitive: true);

        matcher.TryMatch("atm", patterns, out _).Should().BeFalse();
    }

    [Fact]
    public void TryMatch_ExactPattern_DoesNotMatchDifferentCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("atm")], caseSensitive: false);

        matcher.TryMatch("atm-core", patterns, out _).Should().BeFalse();
    }

    [Fact]
    public void TryMatch_PrefixPattern_DoesNotMatchDifferentCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("atm*")], caseSensitive: false);

        matcher.TryMatch("core-atm", patterns, out _).Should().BeFalse();
    }

    [Fact]
    public void TryMatch_SuffixPattern_DoesNotMatchDifferentCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("*atm")], caseSensitive: false);

        matcher.TryMatch("atm-core", patterns, out _).Should().BeFalse();
    }

    [Fact]
    public void TryMatch_SubstringPattern_DoesNotMatchDifferentCandidate()
    {
        var matcher = new ForbiddenPatternMatcher();
        var patterns = matcher.Compile([new ForbiddenPattern("*atm*")], caseSensitive: false);

        matcher.TryMatch("agent-core", patterns, out _).Should().BeFalse();
    }
}
