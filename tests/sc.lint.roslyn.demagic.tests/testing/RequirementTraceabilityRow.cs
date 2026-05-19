namespace sc.lint.roslyn.demagic.tests.testing;

public sealed record RequirementTraceabilityRow(
    string RequirementId,
    string RuleId,
    string SampleFile,
    string OwningTestMethod,
    string ValidationMode,
    AnalyzerSampleKind SampleKind);
