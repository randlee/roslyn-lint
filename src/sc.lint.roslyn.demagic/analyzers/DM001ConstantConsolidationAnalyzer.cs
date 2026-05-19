namespace sc.lint.roslyn.demagic.analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using sc.lint.roslyn.demagic.configuration;
using sc.lint.roslyn.demagic.diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DM001ConstantConsolidationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = DeMagicDiagnosticDescriptors.Dm001Id;

    private static readonly DiagnosticDescriptor Rule = new(
        DeMagicDiagnosticDescriptors.Dm001Id,
        DeMagicDiagnosticDescriptors.Dm001Title,
        DeMagicDiagnosticDescriptors.Dm001MessageFormat,
        DeMagicDiagnosticDescriptors.Dm001Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DeMagicDiagnosticDescriptors.Dm001Description);

    private readonly IDeMagicConfigLoader configLoader;

    public DM001ConstantConsolidationAnalyzer()
        : this(new DeMagicConfigLoader())
    {
    }

    internal DM001ConstantConsolidationAnalyzer(IDeMagicConfigLoader configLoader)
    {
        this.configLoader = configLoader;
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(InitializeCompilation);
    }

    private void InitializeCompilation(CompilationStartAnalysisContext context)
    {
        if (!configLoader.TryLoad(context.Options.AdditionalFiles, out var config, out _))
            return;

        if (!config.Dm001.Enabled || string.IsNullOrWhiteSpace(config.Dm001.DesignatedFile))
            return;

        var designatedFile = config.Dm001.DesignatedFile!;
        var designatedClass = string.IsNullOrWhiteSpace(config.Dm001.DesignatedClass)
            ? null
            : config.Dm001.DesignatedClass;
        var effectiveSeverity = DeMagicDiagnosticDescriptors.GetEffectiveSeverity(config.Dm001.Severity);

        context.RegisterSymbolAction(
            symbolContext => AnalyzeField(symbolContext, designatedFile, designatedClass, effectiveSeverity),
            SymbolKind.Field);
    }

    private static void AnalyzeField(
        SymbolAnalysisContext context,
        string designatedFile,
        string? designatedClass,
        DiagnosticSeverity effectiveSeverity)
    {
        var field = (IFieldSymbol)context.Symbol;
        if (!field.IsConst)
            return;

        if (field.DeclaredAccessibility is not Accessibility.Public and not Accessibility.Internal)
            return;

        var declarator = field.DeclaringSyntaxReferences
            .Select(reference => reference.GetSyntax(context.CancellationToken))
            .OfType<VariableDeclaratorSyntax>()
            .FirstOrDefault();
        if (declarator is null)
            return;

        var location = declarator.Identifier.GetLocation();
        var sourcePath = location.SourceTree?.FilePath;
        if (string.IsNullOrWhiteSpace(sourcePath))
            return;

        var isInDesignatedFile = string.Equals(
            Path.GetFileName(sourcePath),
            designatedFile,
            StringComparison.OrdinalIgnoreCase);
        if (!isInDesignatedFile)
        {
            Report(context, location, field, designatedFile, effectiveSeverity);
            return;
        }

        if (designatedClass is null)
            return;

        if (field.ContainingType is null)
            return;

        if (!string.Equals(field.ContainingType.Name, designatedClass, StringComparison.OrdinalIgnoreCase))
            Report(context, location, field, designatedFile, effectiveSeverity);
    }

    private static void Report(
        SymbolAnalysisContext context,
        Location location,
        IFieldSymbol field,
        string designatedFile,
        DiagnosticSeverity effectiveSeverity)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            location,
            effectiveSeverity,
            additionalLocations: null,
            properties: null,
            messageArgs: [field.Name, designatedFile]));
    }
}
