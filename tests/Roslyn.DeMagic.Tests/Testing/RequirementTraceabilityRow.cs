namespace Roslyn.DeMagic.Tests.Testing;

public sealed record RequirementTraceabilityRow(
    string RequirementId,
    string RuleId,
    string SampleFile,
    string OwningTestMethod,
    string ValidationMode,
    AnalyzerSampleKind SampleKind);
