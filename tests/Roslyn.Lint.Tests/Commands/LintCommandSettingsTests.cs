namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Roslyn.Lint.Commands;
using Spectre.Console.Cli;
using Xunit;

public sealed class LintCommandSettingsTests
{
    [Fact]
    public void Validate_EmptyPath_ReturnsError()
    {
        var settings = new LintCommand.Settings { Path = "" };

        var result = settings.Validate();

        result.Successful.Should().BeFalse();
        result.Message.Should().Contain("Path is required");
    }

    [Fact]
    public void Validate_JsonFlag_Succeeds()
    {
        var settings = new LintCommand.Settings { Path = "src/", Json = true };

        var result = settings.Validate();

        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void Validate_ValidPath_DefaultSettings_Succeeds()
    {
        var settings = new LintCommand.Settings { Path = "src/" };

        var result = settings.Validate();

        result.Successful.Should().BeTrue();
    }
}
