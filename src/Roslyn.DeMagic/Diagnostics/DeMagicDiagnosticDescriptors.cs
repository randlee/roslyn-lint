namespace Roslyn.DeMagic.Diagnostics;

using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Configuration;

internal static class DeMagicDiagnosticDescriptors
{
    public const string Dm001Id = "DM001";
    public const string Dm002Id = "DM002";

    public const string Dm001Category = "roslyn-lint.Organization";
    public const string Dm002Category = "roslyn-lint.DomainBoundary";

    public const string Dm001Title = "Public/internal constant must be consolidated";
    public const string Dm002Title = "Forbidden string literal";

    public const string Dm001MessageFormat =
        "Constant '{0}' must be declared in '{1}' to keep shared constants consolidated";

    public const string Dm002MessageFormat =
        "String literal '{0}' matches forbidden pattern '{1}' and is not permitted in this codebase";

    public const string Dm001Description =
        "DM001 enforces designated-file consolidation for public and internal constants.";

    public const string Dm002Description =
        "DM002 blocks configured domain-specific string literals.";

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
