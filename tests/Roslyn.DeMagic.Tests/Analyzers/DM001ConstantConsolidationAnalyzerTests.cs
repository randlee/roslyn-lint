namespace Roslyn.DeMagic.Tests.Analyzers;

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Analyzers;
using Roslyn.DeMagic.Tests.Testing;
using Xunit;

public sealed class DM001ConstantConsolidationAnalyzerTests
{
    [Fact]
    public async Task PublicConst_OutsideDesignatedFile_ReportsDiagnostic()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/PublicConstOutsideDesignatedFile.cs",
            EnabledWarningConfig);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM001ConstantConsolidationAnalyzer.DiagnosticId);
        diagnostics[0].Severity.Should().Be(DiagnosticSeverity.Warning);
        diagnostics[0].GetMessage().Should().Contain("DefaultEndpoint").And.Contain("Constants.cs");
    }

    [Fact]
    public async Task InternalConst_OutsideDesignatedFile_ReportsDiagnostic()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/InternalConstOutsideDesignatedFile.cs",
            EnabledWarningConfig);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM001ConstantConsolidationAnalyzer.DiagnosticId);
    }

    [Fact]
    public async Task Const_InDesignatedFile_DoesNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/DesignatedFileCompliantConst.cs",
            """
            [dm001]
            enabled = true
            severity = "warning"
            designated_file = "DesignatedFileCompliantConst.cs"
            designated_class = "Constants"
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task DesignatedClassMismatch_ReportsDiagnostic()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/DesignatedClassMismatch.cs",
            """
            [dm001]
            enabled = true
            severity = "warning"
            designated_file = "DesignatedClassMismatch.cs"
            designated_class = "Constants"
            """);

        diagnostics.Should().ContainSingle();
        diagnostics[0].GetMessage().Should().Contain("RetryCount");
    }

    [Fact]
    public async Task PrivateAndProtectedConsts_DoNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/PrivateProtectedIgnored.cs",
            EnabledWarningConfig);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task LocalConst_DoesNotReport()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/LocalConstIgnored.cs",
            EnabledWarningConfig);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task MissingConfig_FailsClosed()
    {
        var diagnostics = await AnalyzerTestHarness.GetDiagnosticsAsync(
            new DM001ConstantConsolidationAnalyzer(),
            AnalyzerTestHarness.LoadFixture("DM001/PublicConstOutsideDesignatedFile.cs"),
            "/repo/DM001/PublicConstOutsideDesignatedFile.cs");

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task InvalidConfig_FailsClosed()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/PublicConstOutsideDesignatedFile.cs",
            """
            [dm001]
            enabled = true
            severity = "loud"
            designated_file = "Constants.cs"
            designated_class = "Constants"
            """);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public async Task Severity_ComesFromConfig()
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/PublicConstOutsideDesignatedFile.cs",
            """
            [dm001]
            enabled = true
            severity = "error"
            designated_file = "Constants.cs"
            designated_class = "Constants"
            """);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Severity.Should().Be(DiagnosticSeverity.Error);
    }

    [Fact]
    public async Task PragmaSuppression_Works()
    {
        var unsuppressedCode = AnalyzerTestHarness.LoadFixture("DM001/SuppressedConst.cs")
            .Replace("#pragma warning disable DM001", string.Empty, StringComparison.Ordinal)
            .Replace("#pragma warning restore DM001", string.Empty, StringComparison.Ordinal)
            .Trim();

        var unsuppressedDiagnostics = await AnalyzerTestHarness.GetDiagnosticsAsync(
            new DM001ConstantConsolidationAnalyzer(),
            unsuppressedCode,
            "/repo/DM001/SuppressedConst.cs",
            new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", EnabledWarningConfig));
        unsuppressedDiagnostics.Should().ContainSingle();

        var diagnostics = await GetDiagnosticsAsync(
            "DM001/SuppressedConst.cs",
            EnabledWarningConfig);

        diagnostics.Should().BeEmpty();
    }

    private const string EnabledWarningConfig =
        """
        [dm001]
        enabled = true
        severity = "warning"
        designated_file = "Constants.cs"
        designated_class = "Constants"
        """;

    private static Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(string fixturePath, string config)
    {
        return AnalyzerTestHarness.GetDiagnosticsAsync(
            new DM001ConstantConsolidationAnalyzer(),
            AnalyzerTestHarness.LoadFixture(fixturePath),
            "/repo/" + fixturePath.Replace('\\', '/'),
            new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", config));
    }
}
