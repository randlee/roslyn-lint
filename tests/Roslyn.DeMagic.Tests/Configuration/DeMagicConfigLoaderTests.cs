namespace Roslyn.DeMagic.Tests.Configuration;

using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Roslyn.DeMagic.Configuration;
using Xunit;

public sealed class DeMagicConfigLoaderTests
{
    [Fact]
    public void TryLoad_ConfigSrcToml_ParsesExpectedSettings()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(new TestAdditionalText(
            "/repo/.roslyn-lint/config-src.toml",
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
    public void TryLoad_CaseSensitiveTrue_ParsesExpectedSettings()
    {
        var loader = new DeMagicConfigLoader();
        var additionalFiles = ImmutableArray.Create<AdditionalText>(new TestAdditionalText(
            "/repo/.roslyn-lint/config-test.toml",
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
            "/repo/.roslyn-lint/config-test.toml",
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

    private sealed class TestAdditionalText(string path, string content) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText? GetText(CancellationToken cancellationToken = default)
            => SourceText.From(content);
    }

    private sealed class ThrowingSelector(string message) : IAdditionalFileConfigSelector
    {
        public AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles)
            => throw new InvalidOperationException(message);
    }

    private sealed class StubSelector(string content) : IAdditionalFileConfigSelector
    {
        public AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles)
            => new("/repo/.roslyn-lint/config-test.toml", content);
    }

    private sealed class ThrowingParser(string message) : ITomlConfigParser
    {
        public bool TryParse(string content, out DeMagicConfig config, out ImmutableArray<string> errors)
            => throw new InvalidOperationException(message);
    }
}
