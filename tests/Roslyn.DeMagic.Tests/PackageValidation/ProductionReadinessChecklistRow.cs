namespace Roslyn.DeMagic.Tests.PackageValidation;

public sealed record ProductionReadinessChecklistRow(
    string RequirementArea,
    string Artifact,
    string ValidationCommand,
    string Owner,
    string StatusEvidence);
