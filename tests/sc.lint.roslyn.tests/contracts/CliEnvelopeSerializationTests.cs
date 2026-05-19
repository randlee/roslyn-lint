namespace sc.lint.roslyn.tests.contracts;

using FluentAssertions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.serialization;
using Xunit;

public sealed class CliEnvelopeSerializationTests
{
    [Fact]
    public async Task SuccessEnvelope_SerializesStableFields()
    {
        var writer = new JsonEnvelopeWriter();
        using var output = new StringWriter();

        await writer.WriteAsync(
            output,
            CliEnvelope<VersionResult>.Success("version", new VersionResult("sc-lint-roslyn", "1.2.3")),
            CancellationToken.None);

        var json = output.ToString();
        json.Should().Contain("\"ok\": true");
        json.Should().Contain("\"command\": \"version\"");
        json.Should().Contain("\"cli\": \"sc-lint-roslyn\"");
        json.Should().NotContain("\"error\"");
        json.Should().NotContain("\"diagnostics\"");
    }

    [Fact]
    public async Task ErrorEnvelope_SerializesTypedErrorFields()
    {
        var writer = new JsonEnvelopeWriter();
        using var output = new StringWriter();

        await writer.WriteAsync(
            output,
            CliEnvelope<object>.Failure(
                "root",
                new CliError(
                    CliErrorKind.Usage,
                    "CLI.USAGE_ERROR",
                    "A command is required.",
                    suggestedAction: "Use --help.")),
            CancellationToken.None);

        var json = output.ToString();
        json.Should().Contain("\"ok\": false");
        json.Should().Contain("\"kind\": \"usage\"");
        json.Should().Contain("\"code\": \"CLI.USAGE_ERROR\"");
        json.Should().Contain("\"suggested_action\": \"Use --help.\"");
        json.Should().NotContain("\"data\"");
        json.Should().NotContain("\"details\"");
        json.Should().NotContain("\"diagnostics\"");
    }
}
