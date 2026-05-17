namespace Roslyn.DeMagic.Tests.Analyzers;

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Analyzers;
using Roslyn.DeMagic.Tests.Testing;
using Xunit;

public sealed class DM002ForbiddenStringLiteralAnalyzerTests
{
    [Fact]
    public async Task ExactMatch_InConstField_ReportsDiagnostic()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/ExactMatchConstField.cs",
            """
            [dm002]
            enabled = true
            severity = "error"
            forbidden = [
              "atm"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM002ForbiddenStringLiteralAnalyzer.DiagnosticId);
        diagnostics[0].Severity.Should().Be(DiagnosticSeverity.Error);
        diagnostics[0].GetMessage().Should().Contain("atm").And.Contain("forbidden pattern");
    }

    [Theory]
    [InlineData("DM002/PrefixMethodArgument.cs", "atm*", DiagnosticSeverity.Warning)]
    [InlineData("DM002/SuffixComparison.cs", "*atm", DiagnosticSeverity.Warning)]
    [InlineData("DM002/SubstringReturnValue.cs", "*atm*", DiagnosticSeverity.Warning)]
    public async Task PatternForms_ReportExpectedDiagnostics(
        string fixturePath,
        string pattern,
        DiagnosticSeverity expectedSeverity)
    {
        var diagnostics = await GetDiagnosticsAsync(
            fixturePath,
            $$"""
            [dm002]
            enabled = true
            severity = "warning"
            forbidden = [
              "{{pattern}}"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM002ForbiddenStringLiteralAnalyzer.DiagnosticId);
        diagnostics[0].Severity.Should().Be(expectedSeverity);
        diagnostics[0].GetMessage().Should().Contain(pattern);
    }

    [Fact]
    public async Task AttributeArgument_ReportsDiagnostic()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/AttributeArgument.cs",
            """
            [dm002]
            enabled = true
            severity = "info"
            forbidden = [
              "*atm*"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Severity.Should().Be(DiagnosticSeverity.Info);
    }

    [Fact]
    public async Task CaseSensitiveConfig_DoesNotMatchDifferentCasing()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/CaseSensitiveMismatch.cs",
            """
            [dm002]
            enabled = true
            severity = "error"
            forbidden = [
              "ATM"
            ]
            case_sensitive = true
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task DisabledRule_DoesNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/ExactMatchConstField.cs",
            """
            [dm002]
            enabled = false
            severity = "error"
            forbidden = [
              "atm"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task InvalidConfig_FailsClosed()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/ExactMatchConstField.cs",
            """
            [dm002]
            enabled = true
            severity = "loud"
            forbidden = [
              "atm"
            ]
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task InterpolatedStringHole_DoesNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/InterpolatedHole.cs",
            """
            [dm002]
            enabled = true
            severity = "error"
            forbidden = [
              "atm"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task NonMatchingLiteral_DoesNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM002/NonMatchingLiteral.cs",
            """
            [dm002]
            enabled = true
            severity = "warning"
            forbidden = [
              "atm",
              "atm*",
              "*atm*",
              "*atm"
            ]
            case_sensitive = false
            """);

        diagnostics.Should().BeEmpty();
    }

    private static Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(string fixturePath, string config)
    {
        return AnalyzerTestHarness.GetDiagnosticsAsync(
            new DM002ForbiddenStringLiteralAnalyzer(),
            AnalyzerTestHarness.LoadFixture(fixturePath),
            "/repo/" + fixturePath.Replace('\\', '/'),
            new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", config));
    }
}
