namespace Roslyn.Lint.Tests.Commands;

using FluentAssertions;
using Xunit;

public sealed class VersionCommandTests
{
    [Fact]
    public async Task Version_WithJson_ReturnsStableEnvelope()
    {
        var result = await CliTestHost.InvokeAsync("version", "--json");

        result.ExitCode.Should().Be(0);
        result.StdErr.Should().BeEmpty();

        var envelope = CliTestHost.ParseJsonObject(result.StdOut);
        envelope["ok"]!.GetValue<bool>().Should().BeTrue();
        envelope["command"]!.GetValue<string>().Should().Be("version");
        envelope["data"]!["cli"]!.GetValue<string>().Should().Be("roslyn-lint");
        envelope["data"]!["version"]!.GetValue<string>().Should().NotBeNullOrWhiteSpace();
    }
}
