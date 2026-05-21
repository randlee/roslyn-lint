namespace sc.lint.roslyn.demagic.diagnostics;

using Microsoft.CodeAnalysis;
using sc.lint.roslyn.demagic;
using sc.lint.roslyn.demagic.configuration;

public static class DeMagicDiagnosticDescriptors
{
    public static DiagnosticSeverity GetEffectiveSeverity(ConfiguredSeverity severity)
    {
        return severity switch
        {
            ConfiguredSeverity.Hidden => DiagnosticSeverity.Hidden,
            ConfiguredSeverity.Info => DiagnosticSeverity.Info,
            ConfiguredSeverity.Warning => DiagnosticSeverity.Warning,
            ConfiguredSeverity.Error => DiagnosticSeverity.Error,
            _ => DiagnosticSeverity.Warning,
        };
    }
}
