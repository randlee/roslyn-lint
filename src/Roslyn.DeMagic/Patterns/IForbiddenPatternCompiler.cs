namespace Roslyn.DeMagic.Patterns;

using System.Collections.Immutable;

public interface IForbiddenPatternCompiler
{
    ImmutableArray<CompiledForbiddenPattern> Compile(
        ImmutableArray<string> patterns,
        bool caseSensitive);
}
