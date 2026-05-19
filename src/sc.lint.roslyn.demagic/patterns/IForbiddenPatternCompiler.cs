namespace sc.lint.roslyn.demagic.patterns;

using System.Collections.Immutable;

public interface IForbiddenPatternCompiler
{
    ImmutableArray<CompiledForbiddenPattern> Compile(
        ImmutableArray<ForbiddenPattern> patterns,
        bool caseSensitive);

    CompiledForbiddenPattern Compile(ForbiddenPattern pattern, bool caseSensitive);

    bool TryMatch(
        string candidate,
        ImmutableArray<CompiledForbiddenPattern> patterns,
        out CompiledForbiddenPattern matchedPattern);
}
