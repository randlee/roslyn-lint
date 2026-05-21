namespace sc.lint.roslyn.demagic.analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using sc.lint.roslyn.demagic;
using sc.lint.roslyn.demagic.configuration;
using sc.lint.roslyn.demagic.diagnostics;
using sc.lint.roslyn.demagic.patterns;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DM002ForbiddenStringLiteralAnalyzer : DiagnosticAnalyzer
{
    public static readonly string DiagnosticId = DeMagicConstants.Dm002Id;

    private static readonly DiagnosticDescriptor Rule = new(
        DeMagicConstants.Dm002Id,
        DeMagicConstants.Dm002Title,
        DeMagicConstants.Dm002MessageFormat,
        DeMagicConstants.Dm002Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: DeMagicConstants.Dm002Description);

    private readonly IDeMagicConfigLoader configLoader;
    private readonly IForbiddenPatternCompiler patternCompiler;

    public DM002ForbiddenStringLiteralAnalyzer()
        : this(new DeMagicConfigLoader(), new ForbiddenPatternMatcher())
    {
    }

    internal DM002ForbiddenStringLiteralAnalyzer(
        IDeMagicConfigLoader configLoader,
        IForbiddenPatternCompiler patternCompiler)
    {
        this.configLoader = configLoader;
        this.patternCompiler = patternCompiler;
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
        if (!configLoader.TryLoad(context.Options.AdditionalFiles, out var config, out var errors))
            return;

        if (errors.Length > 0 || !config.Dm002.Enabled || config.Dm002.ForbiddenPatterns.IsDefaultOrEmpty)
            return;

        var compiledPatterns = patternCompiler.Compile(
            config.Dm002.ForbiddenPatterns,
            config.Dm002.CaseSensitive);
        if (compiledPatterns.IsDefaultOrEmpty)
            return;

        var effectiveSeverity = DeMagicDiagnosticDescriptors.GetEffectiveSeverity(config.Dm002.Severity);
        context.RegisterSyntaxNodeAction(
            syntaxContext => AnalyzeLiteral(syntaxContext, effectiveSeverity, compiledPatterns, patternCompiler),
            SyntaxKind.StringLiteralExpression);
    }

    private static void AnalyzeLiteral(
        SyntaxNodeAnalysisContext context,
        DiagnosticSeverity effectiveSeverity,
        ImmutableArray<CompiledForbiddenPattern> compiledPatterns,
        IForbiddenPatternCompiler patternCompiler)
    {
        var literal = (LiteralExpressionSyntax)context.Node;
        if (literal.Ancestors().OfType<InterpolationSyntax>().Any())
            return;

        var value = literal.Token.ValueText;
        if (!patternCompiler.TryMatch(value, compiledPatterns, out var matchedPattern))
            return;

        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            literal.GetLocation(),
            effectiveSeverity,
            additionalLocations: null,
            properties: null,
            messageArgs: [value, matchedPattern.RawValue]));
    }
}
