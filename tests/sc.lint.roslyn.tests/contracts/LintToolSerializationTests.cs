namespace sc.lint.roslyn.tests.contracts;

using System.Text.Json.Nodes;
using FluentAssertions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.serialization;
using Xunit;

public sealed class LintToolSerializationTests
{
    [Fact]
    public async Task LintToolEnvelope_SerializesFindingsUnderData()
    {
        var writer = new JsonEnvelopeWriter();
        using var output = new StringWriter();

        await writer.WriteAsync(
            output,
            CliEnvelope<LintToolResult>.Success(
                "lint.demagic",
                new LintToolResult(
                    "demagic",
                    "/repo",
                    "findings",
                    1,
                    [new LintFinding("DM002", "warning", "cat", "msg", "file.cs", 1, 1)])),
            CancellationToken.None);

        var envelope = JsonNode.Parse(output.ToString())!.AsObject();
        envelope["findings"].Should().BeNull();
        envelope["data"]!["findings"]!.AsArray().Should().HaveCount(1);
        envelope["data"]!["findingCount"]!.GetValue<int>().Should().Be(1);
    }
}
