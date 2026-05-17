namespace Roslyn.DeMagic.Patterns;

using System.Collections.Immutable;

public sealed class ForbiddenPatternMatcher : IForbiddenPatternCompiler
{
    public ImmutableArray<CompiledForbiddenPattern> Compile(
        ImmutableArray<string> patterns,
        bool caseSensitive)
    {
        var builder = ImmutableArray.CreateBuilder<CompiledForbiddenPattern>(patterns.Length);
        foreach (var pattern in patterns)
        {
            builder.Add(Compile(pattern, caseSensitive));
        }

        return builder.MoveToImmutable();
    }

    public CompiledForbiddenPattern Compile(string rawPattern, bool caseSensitive)
    {
        var kind = GetKind(rawPattern);
        var matchValue = kind switch
        {
            ForbiddenPatternKind.Exact => rawPattern,
            ForbiddenPatternKind.Prefix => rawPattern.Substring(0, rawPattern.Length - 1),
            ForbiddenPatternKind.Suffix => rawPattern.Substring(1),
            ForbiddenPatternKind.Substring => rawPattern.Substring(1, rawPattern.Length - 2),
            _ => rawPattern,
        };

        return new CompiledForbiddenPattern(rawPattern, matchValue, kind, caseSensitive);
    }

    public bool TryMatch(
        string candidate,
        ImmutableArray<CompiledForbiddenPattern> patterns,
        out CompiledForbiddenPattern matchedPattern)
    {
        foreach (var pattern in patterns)
        {
            if (IsMatch(candidate, pattern))
            {
                matchedPattern = pattern;
                return true;
            }
        }

        matchedPattern = default;
        return false;
    }

    private static bool IsMatch(string candidate, CompiledForbiddenPattern pattern)
    {
        var comparison = pattern.CaseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        return pattern.Kind switch
        {
            ForbiddenPatternKind.Exact => string.Equals(candidate, pattern.MatchValue, comparison),
            ForbiddenPatternKind.Prefix => candidate.StartsWith(pattern.MatchValue, comparison),
            ForbiddenPatternKind.Suffix => candidate.EndsWith(pattern.MatchValue, comparison),
            ForbiddenPatternKind.Substring => candidate.IndexOf(pattern.MatchValue, comparison) >= 0,
            _ => false,
        };
    }

    private static ForbiddenPatternKind GetKind(string rawPattern)
    {
        var startsWithWildcard = rawPattern.StartsWith("*", StringComparison.Ordinal);
        var endsWithWildcard = rawPattern.EndsWith("*", StringComparison.Ordinal);

        if (startsWithWildcard && endsWithWildcard && rawPattern.Length > 2)
            return ForbiddenPatternKind.Substring;
        if (startsWithWildcard && rawPattern.Length > 1)
            return ForbiddenPatternKind.Suffix;
        if (endsWithWildcard && rawPattern.Length > 1)
            return ForbiddenPatternKind.Prefix;

        return ForbiddenPatternKind.Exact;
    }
}
