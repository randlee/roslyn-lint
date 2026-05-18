namespace Roslyn.DeMagic.Tests.Analyzers;

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Analyzers;
using Roslyn.DeMagic.Tests.Testing;
using Xunit;

public sealed class DM001ConstantConsolidationAnalyzerTests
{
    private static readonly TheoryData<string, string, ExpectedDiagnostic> PositiveCases =
        new()
        {
            {
                "DM001/PublicConstOutsideDesignatedFile.cs",
                EnabledWarningConfig,
                new ExpectedDiagnostic(
                    DM001ConstantConsolidationAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "DefaultEndpoint",
                    "Constants.cs")
            },
            {
                "DM001/InternalConstOutsideDesignatedFile.cs",
                EnabledWarningConfig,
                new ExpectedDiagnostic(
                    DM001ConstantConsolidationAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "StatusRoute",
                    "Constants.cs")
            },
            {
                "DM001/DesignatedClassMismatch.cs",
                DesignatedClassConfig,
                new ExpectedDiagnostic(
                    DM001ConstantConsolidationAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "RetryCount",
                    "DesignatedClassMismatch.cs")
            },
        };

    private static readonly TheoryData<string, string?> NegativeCases =
        new()
        {
            { "DM001/DesignatedFileCompliantConst.cs", CompliantConfig },
            { "DM001/DesignatedFileCompliantConst.cs", CaseInsensitiveClassConfig },
            { "DM001/PrivateProtectedIgnored.cs", EnabledWarningConfig },
            { "DM001/LocalConstIgnored.cs", EnabledWarningConfig },
            { "DM001/MissingConfigNoDiagnostics.cs", null },
            { "DM001/PublicConstOutsideDesignatedFile.cs", DisabledConfig },
            { "DM001/PublicConstOutsideDesignatedFile.cs", InvalidSeverityConfig },
        };

    private static readonly RequirementTraceabilityRow[] TraceabilityRows =
    [
        new("REQ-DM001-001", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM001-001", "DM001", "DM001/InternalConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM001-002", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM001-002", "DM001", "DM001/DesignatedFileCompliantConst.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.Negative),
        new("REQ-DM001-003", "DM001", "DM001/DesignatedClassMismatch.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM001-003", "DM001", "DM001/DesignatedFileCompliantConst.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.Negative),
        new("REQ-DM001-004", "DM001", "DM001/PrivateProtectedIgnored.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.Negative),
        new("REQ-DM001-005", "DM001", "DM001/LocalConstIgnored.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.Negative),
        new("REQ-DM001-006", "DM001", "DM001/SuppressedConst.cs", nameof(PragmaSuppression_Works), "suppressed", AnalyzerSampleKind.Suppression),
        new("REQ-DM-CONFIG-005", "DM001", "DM001/MissingConfigNoDiagnostics.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-CONFIG-008", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-DIAG-001", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-DIAG-002", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-DIAG-003", "DM001", "DM001/SeverityFromConfig.cs", nameof(SeverityFromConfig_UsesConfiguredSeverity), "diagnostic", AnalyzerSampleKind.Severity),
        new("REQ-DM-DIAG-005", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-TEST-001", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-TEST-002", "DM001", "DM001/SeverityFromConfig.cs", nameof(SeverityFromConfig_UsesConfiguredSeverity), "diagnostic", AnalyzerSampleKind.Severity),
        new("REQ-DM-TEST-002", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-TEST-003", "DM001", "DM001/MissingConfigNoDiagnostics.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-TEST-006", "DM001", "DM001/SuppressedConst.cs", nameof(PragmaSuppression_Works), "suppressed", AnalyzerSampleKind.Suppression),
        new("REQ-DM-TEST-008", "DM001", "DM001/PublicConstOutsideDesignatedFile.cs", nameof(CoverageMatrix_CoversApprovedRequirements), "traceability", AnalyzerSampleKind.CornerCase),
    ];

    [Theory]
    [MemberData(nameof(GetPositiveCases))]
    public async Task PositiveSamples_ReportExpectedDiagnostics(
        string fixturePath,
        string config,
        ExpectedDiagnostic expected)
    {
        var diagnostics = await GetDiagnosticsAsync(fixturePath, config);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(expected.Id);
        diagnostics[0].Severity.Should().Be(expected.Severity);
        diagnostics[0].Descriptor.Category.Should().Be("roslyn-lint.Organization");
        foreach (var fragment in expected.MessageSubstrings)
        {
            diagnostics[0].GetMessage().Should().Contain(fragment);
        }
    }

    [Theory]
    [MemberData(nameof(GetNegativeCases))]
    public async Task NegativeAndConfigSamples_DoNotReport(string fixturePath, string? config)
    {
        var diagnostics = await GetDiagnosticsAsync(fixturePath, config);

        diagnostics.Should().BeEmpty();
    }

    [Theory]
    [InlineData("hidden", DiagnosticSeverity.Hidden)]
    [InlineData("info", DiagnosticSeverity.Info)]
    [InlineData("warning", DiagnosticSeverity.Warning)]
    [InlineData("error", DiagnosticSeverity.Error)]
    public async Task SeverityFromConfig_UsesConfiguredSeverity(string configuredSeverity, DiagnosticSeverity expectedSeverity)
    {
        var diagnostics = await GetDiagnosticsAsync(
            "DM001/PublicConstOutsideDesignatedFile.cs",
            BuildConfig(configuredSeverity, "Constants.cs", "ApiClient"));

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM001ConstantConsolidationAnalyzer.DiagnosticId);
        diagnostics[0].Severity.Should().Be(expectedSeverity);
        diagnostics[0].Descriptor.Category.Should().Be("roslyn-lint.Organization");
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
        unsuppressedDiagnostics[0].Id.Should().Be(DM001ConstantConsolidationAnalyzer.DiagnosticId);

        var diagnostics = await GetDiagnosticsAsync("DM001/SuppressedConst.cs", EnabledWarningConfig);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void CoverageMatrix_CoversApprovedRequirements()
    {
        var expectedRequirementIds = new[]
        {
            "REQ-DM001-001",
            "REQ-DM001-002",
            "REQ-DM001-003",
            "REQ-DM001-004",
            "REQ-DM001-005",
            "REQ-DM001-006",
            "REQ-DM-CONFIG-005",
            "REQ-DM-CONFIG-008",
            "REQ-DM-DIAG-001",
            "REQ-DM-DIAG-002",
            "REQ-DM-DIAG-003",
            "REQ-DM-DIAG-005",
            "REQ-DM-TEST-001",
            "REQ-DM-TEST-002",
            "REQ-DM-TEST-003",
            "REQ-DM-TEST-006",
            "REQ-DM-TEST-008",
        };

        TraceabilityRows.Select(row => (row.RequirementId, row.RuleId, row.SampleFile, row.OwningTestMethod, row.ValidationMode))
            .Distinct()
            .Should().HaveCount(TraceabilityRows.Length);

        TraceabilityRows.Select(row => row.RequirementId)
            .Should().Contain(expectedRequirementIds);
    }

    public static TheoryData<string, string, ExpectedDiagnostic> GetPositiveCases() => PositiveCases;

    public static TheoryData<string, string?> GetNegativeCases() => NegativeCases;

    private const string EnabledWarningConfig =
        """
        [dm001]
        enabled = true
        severity = "warning"
        designated_file = "Constants.cs"
        designated_class = "Constants"
        """;

    private const string DisabledConfig =
        """
        [dm001]
        enabled = false
        severity = "warning"
        designated_file = "Constants.cs"
        designated_class = "Constants"
        """;

    private const string InvalidSeverityConfig =
        """
        [dm001]
        enabled = true
        severity = "loud"
        designated_file = "Constants.cs"
        designated_class = "Constants"
        """;

    private const string DesignatedClassConfig =
        """
        [dm001]
        enabled = true
        severity = "warning"
        designated_file = "DesignatedClassMismatch.cs"
        designated_class = "Constants"
        """;

    private const string CompliantConfig =
        """
        [dm001]
        enabled = true
        severity = "warning"
        // lowercase to verify case-insensitive file-name comparison
        designated_file = "designatedfilecompliantconst.cs"
        designated_class = "Constants"
        """;

    private const string CaseInsensitiveClassConfig =
        """
        [dm001]
        enabled = true
        severity = "warning"
        designated_file = "DesignatedFileCompliantConst.cs"
        designated_class = "constants"
        """;

    private static string BuildConfig(string severity, string designatedFile, string designatedClass)
    {
        return
            $"""
            [dm001]
            enabled = true
            severity = "{severity}"
            designated_file = "{designatedFile}"
            designated_class = "{designatedClass}"
            """;
    }

    private static Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(string fixturePath, string? config)
    {
        var code = AnalyzerTestHarness.LoadFixture(fixturePath);
        var sourcePath = "/repo/" + fixturePath.Replace('\\', '/');

        return config is null
            ? AnalyzerTestHarness.GetDiagnosticsAsync(
                new DM001ConstantConsolidationAnalyzer(),
                code,
                sourcePath)
            : AnalyzerTestHarness.GetDiagnosticsAsync(
                new DM001ConstantConsolidationAnalyzer(),
                code,
                sourcePath,
                new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", config));
    }
}
