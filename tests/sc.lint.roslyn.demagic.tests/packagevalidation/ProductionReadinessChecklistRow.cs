namespace sc.lint.roslyn.demagic.tests.packagevalidation;

public sealed record ProductionReadinessChecklistRow(
    string RequirementArea,
    string Artifact,
    string ValidationCommand,
    string Owner,
    string StatusEvidence);
