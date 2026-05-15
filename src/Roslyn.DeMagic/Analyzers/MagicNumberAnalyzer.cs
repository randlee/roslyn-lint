namespace Roslyn.DeMagic.Analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Detects numeric literals that should be replaced with named constants.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MagicNumberAnalyzer : DiagnosticAnalyzer
{
    /// <summary>Diagnostic ID for magic number violations.</summary>
    public const string DiagnosticId = "DM001";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Magic number detected",
        messageFormat: "Numeric literal '{0}' should be replaced with a named constant",
        category: "Maintainability",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Magic numbers reduce code readability and maintainability. Replace with a named constant.");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.NumericLiteralExpression);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var literal = (LiteralExpressionSyntax)context.Node;
        var token = literal.Token;

        // 0 and 1 are universally acceptable
        if (token.Value is int intVal && intVal is 0 or 1)
            return;
        if (token.Value is double dblVal && dblVal is 0.0 or 1.0)
            return;

        // Skip enum member declarations (defining constants, not using them)
        if (literal.Parent is EqualsValueClauseSyntax { Parent: EnumMemberDeclarationSyntax })
            return;

        // Skip const field declarations (they ARE the named constant)
        if (IsConstDeclaration(literal))
            return;

        // Skip attribute arguments — [MaxLength(255)], [Range(0, 100)], etc.
        if (literal.Ancestors().OfType<AttributeSyntax>().Any())
            return;

        // Skip default parameter values
        if (literal.Parent is EqualsValueClauseSyntax { Parent: ParameterSyntax })
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, literal.GetLocation(), token.ValueText));
    }

    private static bool IsConstDeclaration(LiteralExpressionSyntax literal)
    {
        if (literal.Parent is not EqualsValueClauseSyntax { Parent: VariableDeclaratorSyntax varDecl })
            return false;

        if (varDecl.Parent is VariableDeclarationSyntax { Parent: FieldDeclarationSyntax field })
            return field.Modifiers.Any(SyntaxKind.ConstKeyword);

        if (varDecl.Parent is VariableDeclarationSyntax { Parent: LocalDeclarationStatementSyntax local })
            return local.Modifiers.Any(SyntaxKind.ConstKeyword);

        return false;
    }
}
