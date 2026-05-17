namespace Roslyn.DeMagic.Patterns;

public readonly record struct CompiledForbiddenPattern(
    string RawValue,
    string MatchValue,
    ForbiddenPatternKind Kind,
    bool CaseSensitive);
