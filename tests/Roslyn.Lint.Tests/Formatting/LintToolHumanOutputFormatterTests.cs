namespace Roslyn.Lint.Tests.Formatting;

using FluentAssertions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Formatting;
using Xunit;

public sealed class LintToolHumanOutputFormatterTests
{
    [Fact]
    public async Task WriteAsync_WithZeroFindings_WritesSummaryOnly()
    {
        var formatter = new LintToolHumanOutputFormatter();
        using var writer = new StringWriter();

        await formatter.WriteAsync(
            writer,
            new LintToolResult("demagic", "/repo", "pass", 0, []),
            CancellationToken.None);

        writer.ToString().Should().Be($"demagic: pass (0 findings){Environment.NewLine}");
    }

    [Fact]
    public async Task WriteAsync_WithFindings_WritesSummaryAndDetails()
    {
        var formatter = new LintToolHumanOutputFormatter();
        using var writer = new StringWriter();

        await formatter.WriteAsync(
            writer,
            new LintToolResult(
                "demagic",
                "/repo",
                "fail",
                1,
                [new LintFinding("DM001", "warning", "style", "Avoid magic literals.", "Program.cs", 12, 34)]),
            CancellationToken.None);

        writer.ToString().Should().Be(
            $"demagic: fail (1 findings){Environment.NewLine}" +
            $"Program.cs(12,34): warning DM001 Avoid magic literals.{Environment.NewLine}");
    }
}
