namespace sc.lint.roslyn.demagic.patterns;

public readonly record struct CompiledForbiddenPattern(
    string RawValue,
    string MatchValue,
    ForbiddenPatternKind Kind,
    bool CaseSensitive);
