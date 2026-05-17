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

    [Theory]
    [InlineData("text")]
    [InlineData("json")]
    [InlineData("TEXT")]
    [InlineData("JSON")]
    public void Validate_ValidFormat_Succeeds(string format)
    {
        var settings = new LintCommand.Settings { Path = "src/", Format = format };

        var result = settings.Validate();

        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidFormat_ReturnsError()
    {
        var settings = new LintCommand.Settings { Path = "src/", Format = "xml" };

        var result = settings.Validate();

        result.Successful.Should().BeFalse();
        result.Message.Should().Contain("Invalid format");
    }

    [Fact]
    public void Validate_ValidPath_DefaultFormat_Succeeds()
    {
        var settings = new LintCommand.Settings { Path = "src/" };

        var result = settings.Validate();

        result.Successful.Should().BeTrue();
    }
}
