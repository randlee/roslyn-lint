namespace Roslyn.DeMagic.Tests.Analyzers;

using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.DeMagic.Analyzers;
using Xunit;

public sealed class MagicStringAnalyzerTests
{
    private static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create(
            assemblyName: "test",
            syntaxTrees: [tree],
            references: [],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var analyzer = new MagicStringAnalyzer();
        var withAnalyzers = compilation.WithAnalyzers([analyzer]);
        return await withAnalyzers.GetAnalyzerDiagnosticsAsync();
    }

    [Fact]
    public async Task MagicString_InExpression_ReportsDiagnostic()
    {
        var code = """
            class Foo
            {
                void Bar() { var s = "hello-world"; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().ContainSingle(d => d.Id == MagicStringAnalyzer.DiagnosticId);
    }

    [Fact]
    public async Task EmptyString_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar() { var s = ""; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task SingleCharString_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar() { var s = "x"; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task ConstField_IsAllowed()
    {
        var code = """
            class Foo
            {
                private const string Key = "my-secret-key";
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task ConstLocal_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar()
                {
                    const string prefix = "api-v2";
                }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task AttributeArgument_IsAllowed()
    {
        var code = """
            using System;
            class Foo
            {
                [Obsolete("Use NewMethod instead")]
                void OldMethod() { }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task DefaultParameter_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar(string prefix = "api") { }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }
}
