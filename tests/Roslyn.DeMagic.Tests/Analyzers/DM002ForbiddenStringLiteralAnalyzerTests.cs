namespace Roslyn.DeMagic.Tests.Analyzers;

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Roslyn.DeMagic.Analyzers;
using Roslyn.DeMagic.Tests.Testing;
using Xunit;

public sealed class DM002ForbiddenStringLiteralAnalyzerTests
{
    private static readonly TheoryData<string, string, ExpectedDiagnostic> PositiveCases =
        new()
        {
            {
                "DM002/ExactMatchConstField.cs",
                BuildConfig("error", false, "atm"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Error,
                    "atm",
                    "forbidden pattern")
            },
            {
                "DM002/CaseSensitiveMismatch.cs",
                BuildConfig("warning", false, "ATM"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "ATM")
            },
            {
                "DM002/ExactMatchConstField.cs",
                BuildConfig("warning", true, "atm"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "atm",
                    "forbidden pattern")
            },
            {
                "DM002/PrefixMethodArgument.cs",
                BuildConfig("warning", false, "atm*"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "atm*")
            },
            {
                "DM002/SuffixComparison.cs",
                BuildConfig("warning", false, "*atm"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "*atm")
            },
            {
                "DM002/SubstringReturnValue.cs",
                BuildConfig("warning", false, "*atm*"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "*atm*")
            },
            {
                "DM002/AttributeArgument.cs",
                BuildConfig("info", false, "*atm*"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Info,
                    "*atm*")
            },
            {
                "DM002/SwitchArmLiteral.cs",
                BuildConfig("warning", false, "atm*"),
                new ExpectedDiagnostic(
                    DM002ForbiddenStringLiteralAnalyzer.DiagnosticId,
                    DiagnosticSeverity.Warning,
                    "atm*")
            },
        };

    private static readonly TheoryData<string, string?> NegativeCases =
        new()
        {
            { "DM002/CaseSensitiveMismatch.cs", BuildConfig("error", true, "ATM") },
            { "DM002/NonMatchingLiteral.cs", BuildConfig("warning", false, "atm", "atm*", "*atm*", "*atm") },
            { "DM002/InterpolatedHole.cs", BuildConfig("error", false, "atm") },
            { "DM002/CommentsAndDocumentationIgnored.cs", BuildConfig("warning", false, "*atm*") },
            { "DM002/MissingConfigNoDiagnostics.cs", null },
            {
                "DM002/InvalidConfigNoDiagnostics.cs",
                """
                [dm002]
                enabled = true
                severity = "loud"
                forbidden = [
                  "atm"
                ]
                case_sensitive = false
                """
            },
            { "DM002/ExactMatchConstField.cs", DisabledConfig },
        };

    private static readonly RequirementTraceabilityRow[] TraceabilityRows =
    [
        new("REQ-DM002-001", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-001", "DM002", "DM002/PrefixMethodArgument.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-001", "DM002", "DM002/SuffixComparison.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-001", "DM002", "DM002/SubstringReturnValue.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-002", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-002", "DM002", "DM002/PrefixMethodArgument.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-002", "DM002", "DM002/SuffixComparison.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-002", "DM002", "DM002/SubstringReturnValue.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-003", "DM002", "DM002/CaseSensitiveMismatch.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM002-003", "DM002", "DM002/CaseSensitiveMismatch.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM002-004", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-004", "DM002", "DM002/SuffixComparison.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-004", "DM002", "DM002/PrefixMethodArgument.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-004", "DM002", "DM002/SubstringReturnValue.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-004", "DM002", "DM002/AttributeArgument.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM002-004", "DM002", "DM002/SwitchArmLiteral.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM002-005", "DM002", "DM002/InterpolatedHole.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM002-005", "DM002", "DM002/CommentsAndDocumentationIgnored.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM002-006", "DM002", "DM002/SuppressedLiteral.cs", nameof(PragmaSuppression_Works), "suppressed", AnalyzerSampleKind.Suppression),
        new("REQ-DM-CONFIG-005", "DM002", "DM002/MissingConfigNoDiagnostics.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-CONFIG-008", "DM002", "DM002/InvalidConfigNoDiagnostics.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-DIAG-001", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-DIAG-002", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-DIAG-003", "DM002", "DM002/SeverityFromConfig.cs", nameof(SeverityFromConfig_UsesConfiguredSeverity), "diagnostic", AnalyzerSampleKind.Severity),
        new("REQ-DM-DIAG-005", "DM002", "DM002/ExactMatchConstField.cs", nameof(PositiveSamples_ReportExpectedDiagnostics), "diagnostic", AnalyzerSampleKind.Positive),
        new("REQ-DM-TEST-001", "DM002", "DM002/SuppressedLiteral.cs", nameof(PragmaSuppression_Works), "suppressed", AnalyzerSampleKind.Suppression),
        new("REQ-DM-TEST-002", "DM002", "DM002/SeverityFromConfig.cs", nameof(SeverityFromConfig_UsesConfiguredSeverity), "diagnostic", AnalyzerSampleKind.Severity),
        new("REQ-DM-TEST-002", "DM002", "DM002/ExactMatchConstField.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-TEST-003", "DM002", "DM002/MissingConfigNoDiagnostics.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-TEST-006", "DM002", "DM002/CommentsAndDocumentationIgnored.cs", nameof(NegativeAndConfigSamples_DoNotReport), "no-diagnostic", AnalyzerSampleKind.CornerCase),
        new("REQ-DM-TEST-008", "DM002", "DM002/SwitchArmLiteral.cs", nameof(CoverageMatrix_CoversApprovedRequirements), "traceability", AnalyzerSampleKind.CornerCase),
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
        diagnostics[0].Descriptor.Category.Should().Be("roslyn-lint.DomainBoundary");
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
            "DM002/SeverityFromConfig.cs",
            BuildConfig(configuredSeverity, false, "atm"));

        diagnostics.Should().ContainSingle();
        diagnostics[0].Id.Should().Be(DM002ForbiddenStringLiteralAnalyzer.DiagnosticId);
        diagnostics[0].Severity.Should().Be(expectedSeverity);
        diagnostics[0].Descriptor.Category.Should().Be("roslyn-lint.DomainBoundary");
    }

    [Fact]
    public async Task PragmaSuppression_Works()
    {
        var unsuppressedCode = AnalyzerTestHarness.LoadFixture("DM002/SuppressedLiteral.cs")
            .Replace("#pragma warning disable DM002", string.Empty, StringComparison.Ordinal)
            .Replace("#pragma warning restore DM002", string.Empty, StringComparison.Ordinal)
            .Trim();

        var unsuppressedDiagnostics = await AnalyzerTestHarness.GetDiagnosticsAsync(
            new DM002ForbiddenStringLiteralAnalyzer(),
            unsuppressedCode,
            "/repo/DM002/SuppressedLiteral.cs",
            new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", BuildConfig("error", false, "atm")));

        unsuppressedDiagnostics.Should().ContainSingle();
        unsuppressedDiagnostics[0].Id.Should().Be(DM002ForbiddenStringLiteralAnalyzer.DiagnosticId);

        var diagnostics = await GetDiagnosticsAsync(
            "DM002/SuppressedLiteral.cs",
            BuildConfig("error", false, "atm"));

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void CoverageMatrix_CoversApprovedRequirements()
    {
        var expectedRequirementIds = new[]
        {
            "REQ-DM002-001",
            "REQ-DM002-002",
            "REQ-DM002-003",
            "REQ-DM002-004",
            "REQ-DM002-005",
            "REQ-DM002-006",
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

    private const string DisabledConfig =
        """
        [dm002]
        enabled = false
        severity = "error"
        forbidden = [
          "atm"
        ]
        case_sensitive = false
        """;

    private static string BuildConfig(string severity, bool caseSensitive, params string[] patterns)
    {
        var renderedPatterns = string.Join(
            Environment.NewLine,
            patterns.Select(pattern => $"  \"{pattern}\""));

        return
            $"""
            [dm002]
            enabled = true
            severity = "{severity}"
            forbidden = [
            {renderedPatterns}
            ]
            case_sensitive = {caseSensitive.ToString().ToLowerInvariant()}
            """;
    }

    private static Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(string fixturePath, string? config)
    {
        var code = AnalyzerTestHarness.LoadFixture(fixturePath);
        var sourcePath = "/repo/" + fixturePath.Replace('\\', '/');

        return config is null
            ? AnalyzerTestHarness.GetDiagnosticsAsync(
                new DM002ForbiddenStringLiteralAnalyzer(),
                code,
                sourcePath)
            : AnalyzerTestHarness.GetDiagnosticsAsync(
                new DM002ForbiddenStringLiteralAnalyzer(),
                code,
                sourcePath,
                new TestAdditionalText("/repo/.roslyn-lint/config-src.toml", config));
    }
}
