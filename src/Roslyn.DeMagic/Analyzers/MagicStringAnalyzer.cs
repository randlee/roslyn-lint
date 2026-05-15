namespace Roslyn.DeMagic.Analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Detects string literals that should be replaced with named constants or resource keys.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MagicStringAnalyzer : DiagnosticAnalyzer
{
    /// <summary>Diagnostic ID for magic string violations.</summary>
    public const string DiagnosticId = "DM002";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Magic string detected",
        messageFormat: "String literal '{0}' should be replaced with a named constant",
        category: "Maintainability",
        defaultSeverity: DiagnosticSeverity.Hidden,
        isEnabledByDefault: true,
        description: "Magic strings reduce code readability and make refactoring error-prone. Replace with a named constant.");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.StringLiteralExpression);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var literal = (LiteralExpressionSyntax)context.Node;
        var value = literal.Token.ValueText;

        // Empty strings and single-char strings are typically fine
        if (value.Length <= 1)
            return;

        // Skip const field/local declarations
        if (IsConstDeclaration(literal))
            return;

        // Skip attribute arguments
        if (literal.Ancestors().OfType<AttributeSyntax>().Any())
            return;

        // Skip default parameter values
        if (literal.Parent is EqualsValueClauseSyntax { Parent: ParameterSyntax })
            return;

        // Skip nameof() expressions (they aren't really magic strings)
        if (literal.Ancestors().OfType<InvocationExpressionSyntax>()
                .Any(inv => inv.Expression is IdentifierNameSyntax { Identifier.ValueText: "nameof" }))
            return;

        // Skip string interpolation holes
        if (literal.Parent is InterpolatedStringExpressionSyntax)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, literal.GetLocation(), value));
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
