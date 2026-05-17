namespace Roslyn.DeMagic.Analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.DeMagic.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DM001ConstantConsolidationAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        DeMagicDiagnosticDescriptors.Dm001Id,
        DeMagicDiagnosticDescriptors.Dm001Title,
        DeMagicDiagnosticDescriptors.Dm001MessageFormat,
        DeMagicDiagnosticDescriptors.Dm001Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DeMagicDiagnosticDescriptors.Dm001Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
    }
}
