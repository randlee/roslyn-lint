namespace Roslyn.DeMagic.Tests.Analyzers;

using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.DeMagic.Analyzers;
using Xunit;

public sealed class MagicNumberAnalyzerTests
{
    private static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create(
            assemblyName: "test",
            syntaxTrees: [tree],
            references: [],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var analyzer = new MagicNumberAnalyzer();
        var withAnalyzers = compilation.WithAnalyzers([analyzer]);
        return await withAnalyzers.GetAnalyzerDiagnosticsAsync();
    }

    [Fact]
    public async Task MagicNumber_InExpression_ReportsDiagnostic()
    {
        var code = """
            class Foo
            {
                void Bar() { var x = 42; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().ContainSingle(d => d.Id == MagicNumberAnalyzer.DiagnosticId);
    }

    [Fact]
    public async Task Zero_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar() { var x = 0; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task One_IsAllowed()
    {
        var code = """
            class Foo
            {
                void Bar() { var x = 1; }
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
                private const int MaxRetries = 42;
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
                    const int timeout = 30;
                }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task EnumMember_IsAllowed()
    {
        var code = """
            enum Status
            {
                Active = 1,
                Inactive = 99
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task AttributeArgument_IsAllowed()
    {
        var code = """
            using System.ComponentModel.DataAnnotations;
            class Foo
            {
                [Range(1, 100)]
                public int Value { get; set; }
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
                void Bar(int timeout = 30) { }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task MultipleMagicNumbers_ReportsAll()
    {
        var code = """
            class Foo
            {
                void Bar()
                {
                    var x = 42;
                    var y = 100;
                    var z = 256;
                }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().HaveCount(3).And.OnlyContain(d => d.Id == MagicNumberAnalyzer.DiagnosticId);
    }

    [Fact]
    public async Task DiagnosticMessage_IncludesLiteralValue()
    {
        var code = """
            class Foo
            {
                void Bar() { var x = 99; }
            }
            """;

        var diagnostics = await GetDiagnosticsAsync(code);

        diagnostics.Should().ContainSingle().Which.GetMessage().Should().Contain("99");
    }
}
