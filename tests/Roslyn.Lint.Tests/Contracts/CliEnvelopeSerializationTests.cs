namespace Roslyn.Lint.Tests.Contracts;

using System.Collections.Immutable;
using System.Text.Json;
using FluentAssertions;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Serialization;
using Xunit;

public sealed class CliEnvelopeSerializationTests
{
    [Fact]
    public void Serialize_SuccessEnvelope_UsesStableCamelCaseContract()
    {
        var envelope = CliEnvelope<LintResult>.Ok(
            "lint.run",
            new LintResult(1, ImmutableArray.Create(new LintIssue("DM002", "warning", "Forbidden string", "Example.cs", 3, 9))));

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(envelope, JsonEnvelopeWriter.DefaultOptions));
        var root = document.RootElement;

        root.GetProperty("success").GetBoolean().Should().BeTrue();
        root.GetProperty("operation").GetString().Should().Be("lint.run");
        root.GetProperty("result").GetProperty("count").GetInt32().Should().Be(1);
        root.GetProperty("warnings").ValueKind.Should().Be(JsonValueKind.Array);
        root.TryGetProperty("error", out _).Should().BeFalse();
    }

    [Fact]
    public void Serialize_FailureEnvelope_OmitsResultAndUsesStringErrorKind()
    {
        var envelope = CliEnvelope<LintResult>.Fail(
            "lint.run",
            new CliError(
                CliErrorKind.Validation,
                "CLI.INVALID_PATH",
                "Path does not exist",
                new Dictionary<string, string> { ["path"] = "missing" },
                "Provide an existing path"));

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(envelope, JsonEnvelopeWriter.DefaultOptions));
        var root = document.RootElement;

        root.GetProperty("success").GetBoolean().Should().BeFalse();
        root.GetProperty("operation").GetString().Should().Be("lint.run");
        root.GetProperty("error").GetProperty("kind").GetString().Should().Be("validation");
        root.GetProperty("error").GetProperty("code").GetString().Should().Be("CLI.INVALID_PATH");
        root.TryGetProperty("result", out _).Should().BeFalse();
    }
}
