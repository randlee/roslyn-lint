namespace sc.lint.roslyn.demagic.configuration;

public sealed record Dm001Options(
    bool Enabled,
    ConfiguredSeverity Severity,
    string? DesignatedFile,
    string? DesignatedClass);
