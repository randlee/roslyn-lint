namespace sc.lint.roslyn.demagic.tests.configuration;

using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using sc.lint.roslyn.demagic.configuration;
using sc.lint.roslyn.demagic.tests.testing;
using Xunit;

public sealed class DeMagicConfigLoaderTests
{
    private static readonly RequirementTraceabilityRow[] TraceabilityRows =
    [
        new("REQ-DM-CONFIG-002", "config", ".sc-lint-roslyn/config-src.toml", nameof(TryLoad_ConfigSrcToml_ParsesExpectedSettings), "config-routing", AnalyzerSampleKind.CornerCase),
        new("REQ-DM-CONFIG-003", "config", ".sc-lint-roslyn/config-test.toml", nameof(TryLoad_ConfigTestToml_WinsAndDoesNotMergeSourceConfig), "config-routing", AnalyzerSampleKind.CornerCase),
        new("REQ-DM-CONFIG-003", "config", ".sc-lint-roslyn/config-test.toml", nameof(TryLoad_CaseSensitiveTrue_ParsesExpectedSettings), "config-parsing", AnalyzerSampleKind.CornerCase),
        new("REQ-DM-CONFIG-005", "config", ".sc-lint-roslyn/config-src.toml", nameof(TryLoad_NoConfigFiles_ReturnsDisabledConfig), "disabled-without-config", AnalyzerSampleKind.ConfigFailure),
        new("REQ-DM-CONFIG-008", "config", ".sc-lint-roslyn/config-test.toml", nameof(TryLoad_InvalidSeverity_ReturnsErrors), "disabled-with-error", AnalyzerSampleKind.ConfigFailure),
    ];

    [Fact]
    public void TryLoad_ConfigSrcToml_ParsesExpectedSettings()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(new TestAdditionalText(
            "/repo/.sc-lint-roslyn/config-src.toml",
            """
            [dm001]
            enabled = true
            severity = "warning"
            designated_file = "Constants.cs"
            designated_class = "Constants"

            [dm002]
            enabled = true
            severity = "error"
            forbidden = [
              "atm",
              "atm*",
              "*atm*",
            ]
            case_sensitive = false
            """));

        var success = loader.TryLoad(additionalFiles, out var config, out var errors);

        success.Should().BeTrue();
        errors.Should().BeEmpty();
        config.Dm001.Enabled.Should().BeTrue();
        config.Dm001.DesignatedFile.Should().Be("Constants.cs");
        config.Dm001.DesignatedClass.Should().Be("Constants");
        config.Dm002.ForbiddenPatterns.Select(pattern => pattern.RawValue)
            .Should().ContainInOrder("atm", "atm*", "*atm*");
        config.Dm002.CaseSensitive.Should().BeFalse();
    }

    [Fact]
    public void TryLoad_ConfigTestToml_WinsAndDoesNotMergeSourceConfig()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(
            new TestAdditionalText(
                "/repo/.sc-lint-roslyn/config-src.toml",
                """
                [dm001]
                enabled = true
                severity = "warning"
                designated_file = "SourceConstants.cs"
                designated_class = "SourceConstants"

                [dm002]
                enabled = true
                severity = "error"
                forbidden = [
                  "atm"
                ]
                case_sensitive = false
                """),
            new TestAdditionalText(
                "/repo/.sc-lint-roslyn/config-test.toml",
                """
                [dm002]
                enabled = true
                severity = "info"
                forbidden = [
                  "ATM"
                ]
                case_sensitive = true
                """));

        var success = loader.TryLoad(additionalFiles, out var config, out var errors);

        success.Should().BeTrue();
        errors.Should().BeEmpty();
        config.Dm001.Enabled.Should().BeFalse();
        config.Dm001.DesignatedFile.Should().BeNull();
        config.Dm001.DesignatedClass.Should().BeNull();
        config.Dm002.Enabled.Should().BeTrue();
        config.Dm002.Severity.Should().Be(ConfiguredSeverity.Info);
        config.Dm002.ForbiddenPatterns.Should().ContainSingle().Which.RawValue.Should().Be("ATM");
        config.Dm002.CaseSensitive.Should().BeTrue();
    }

    [Fact]
    public void TryLoad_CaseSensitiveTrue_ParsesExpectedSettings()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(new TestAdditionalText(
            "/repo/.sc-lint-roslyn/config-test.toml",
            """
            [dm001]
            enabled = false

            [dm002]
            enabled = true
            severity = "info"
            forbidden = [
              "ATM"
            ]
            case_sensitive = true
            """));

        var success = loader.TryLoad(additionalFiles, out var config, out var errors);

        success.Should().BeTrue();
        errors.Should().BeEmpty();
        config.Dm001.Enabled.Should().BeFalse();
        config.Dm002.Enabled.Should().BeTrue();
        config.Dm002.ForbiddenPatterns.Should().ContainSingle().Which.RawValue.Should().Be("ATM");
        config.Dm002.CaseSensitive.Should().BeTrue();
    }

    [Fact]
    public void TryLoad_NoConfigFiles_ReturnsDisabledConfig()
    {
        var loader = new DeMagicConfigLoader();

        var success = loader.TryLoad(ImmutableArray<AdditionalText>.Empty, out var config, out var errors);

        success.Should().BeFalse();
        errors.Should().BeEmpty();
        config.Should().Be(DeMagicConfig.Disabled);
    }

    [Fact]
    public void TryLoad_InvalidSeverity_ReturnsErrors()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(new TestAdditionalText(
            "/repo/.sc-lint-roslyn/config-test.toml",
            """
            [dm001]
            enabled = true
            severity = "loud"
            """));

        var success = loader.TryLoad(additionalFiles, out var config, out var errors);

        success.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Contain("Unsupported severity");
        config.Should().Be(DeMagicConfig.Disabled);
    }

    [Fact]
    public void TryLoad_SelectorThrows_ReturnsDisabledConfigAndError()
    {
        var loader = new DeMagicConfigLoader(
            new ThrowingSelector("selector failed"),
            new SimpleTomlConfigParser());

        var success = loader.TryLoad(ImmutableArray<AdditionalText>.Empty, out var config, out var errors);

        success.Should().BeFalse();
        config.Should().Be(DeMagicConfig.Disabled);
        errors.Should().ContainSingle().Which.Should().Contain("selector failed");
    }

    [Fact]
    public void TryLoad_ParserThrows_ReturnsDisabledConfigAndError()
    {
        var loader = new DeMagicConfigLoader(
            new StubSelector("""
            [dm001]
            enabled = true
            """),
            new ThrowingParser("parser failed"));

        var success = loader.TryLoad(ImmutableArray<AdditionalText>.Empty, out var config, out var errors);

        success.Should().BeFalse();
        config.Should().Be(DeMagicConfig.Disabled);
        errors.Should().ContainSingle().Which.Should().Contain("parser failed");
    }

    [Fact]
    public void CoverageMatrix_CoversApprovedRequirements()
    {
        var expectedRequirementIds = new[]
        {
            "REQ-DM-CONFIG-002",
            "REQ-DM-CONFIG-003",
            "REQ-DM-CONFIG-005",
            "REQ-DM-CONFIG-008",
        };

        TraceabilityRows.Select(row => (row.RequirementId, row.RuleId, row.SampleFile, row.OwningTestMethod, row.ValidationMode))
            .Distinct()
            .Should().HaveCount(TraceabilityRows.Length);

        TraceabilityRows.Select(row => row.RequirementId)
            .Should().Contain(expectedRequirementIds);
    }

    private sealed class ThrowingSelector(string message) : IAdditionalFileConfigSelector
    {
        public AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles)
            => throw new InvalidOperationException(message);
    }

    private sealed class StubSelector(string content) : IAdditionalFileConfigSelector
    {
        public AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles)
            => new("/repo/.sc-lint-roslyn/config-test.toml", content);
    }

    private sealed class ThrowingParser(string message) : ITomlConfigParser
    {
        public bool TryParse(string content, out DeMagicConfig config, out ImmutableArray<string> errors)
            => throw new InvalidOperationException(message);
    }
}
