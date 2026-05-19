namespace Roslyn.DeMagic.Configuration;

public sealed record Dm001Options(
    bool Enabled,
    ConfiguredSeverity Severity,
    string? DesignatedFile,
    string? DesignatedClass);
