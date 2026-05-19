using FluentAssertions;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.backends;
using sc.lint.roslyn.dispatch;
using sc.lint.roslyn.serialization;
using Xunit;

namespace sc.lint.roslyn.tests.dispatch;

public sealed class BackendJsonNormalizerTests
{
    [Fact]
    public void NormalizeDelegatedResult_WithSuccessEnvelope_ReturnsViewData()
    {
        var normalizer = new BackendJsonNormalizer();
        var result = new BackendProcessResult(
            "demagic-backend",
            "/repo",
            ["view", "rules", "--json"],
            0,
            """
            {
              "ok": true,
              "command": "view.rules",
              "data": {
                "target": "rules",
                "rules": [
                  {
                    "tool": "demagic",
                    "id": "DM002",
                    "title": "Forbidden string literal",
                    "category": "sc.lint.roslyn.domainboundary",
                    "defaultSeverity": "error",
                    "isEnabledByDefault": true,
                    "messageFormat": "Forbidden string literal",
                    "description": "DM002 blocks configured domain-specific string literals."
                  }
                ]
              }
            }
            """,
            string.Empty);

        var normalized = normalizer.NormalizeDelegatedResult("view.rules", result, RoslynLintJsonContext.Default.ViewResult);

        normalized.IsSuccess.Should().BeTrue();
        normalized.Data.Should().NotBeNull();
        normalized.Data!.Target.Should().Be("rules");
        normalized.Data.Rules.Should().ContainSingle(rule => rule.Id == "DM002");
        normalized.Error.Should().BeNull();
    }

    [Fact]
    public void NormalizeDelegatedResult_WithFailureEnvelope_ReturnsBackendError()
    {
        var normalizer = new BackendJsonNormalizer();
        var result = new BackendProcessResult(
            "demagic-backend",
            "/repo",
            ["view", "rules", "--json"],
            1,
            """
            {
              "ok": false,
              "command": "view.rules",
              "error": {
                "kind": "backend_failure",
                "code": "DM.RULES_FAILURE",
                "message": "rule export failed"
              }
            }
            """,
            "boom");

        var normalized = normalizer.NormalizeDelegatedResult("view.rules", result, RoslynLintJsonContext.Default.ViewResult);

        normalized.IsSuccess.Should().BeFalse();
        normalized.Error.Should().NotBeNull();
        normalized.Error!.Code.Should().Be("DM.RULES_FAILURE");
        normalized.Error.Kind.Should().Be(CliErrorKind.BackendFailure);
    }

    [Fact]
    public void NormalizeDelegatedResult_WithMalformedJsonAndZeroExit_ReturnsProtocolError()
    {
        var normalizer = new BackendJsonNormalizer();
        var result = new BackendProcessResult(
            "demagic-backend",
            "/repo",
            ["view", "rules", "--json"],
            0,
            "not-json",
            string.Empty);

        var normalized = normalizer.NormalizeDelegatedResult("view.rules", result, RoslynLintJsonContext.Default.ViewResult);

        normalized.IsSuccess.Should().BeFalse();
        normalized.Error.Should().NotBeNull();
        normalized.Error!.Code.Should().Be("CLI.BACKEND_PROTOCOL_ERROR");
        normalized.Error.Kind.Should().Be(CliErrorKind.BackendProtocol);
    }

    [Fact]
    public void NormalizeWorkflowFailure_WithDotnetToolUnavailableException_ReturnsCapabilityError()
    {
        var normalizer = new BackendJsonNormalizer();

        var error = normalizer.NormalizeWorkflowFailure(
            "view.rules",
            new DotnetToolUnavailableException(new FileNotFoundException("dotnet")));

        error.Kind.Should().Be(CliErrorKind.Capability);
        error.Code.Should().Be("CLI.CAPABILITY_ERROR");
        error.Details.Should().ContainKey("tool").WhoseValue.Should().Be("dotnet");
    }

    [Fact]
    public void NormalizeWorkflowFailure_WithDotnetCommandFailedException_ReturnsBackendFailure()
    {
        var normalizer = new BackendJsonNormalizer();
        var result = new DotnetCommandResult("/repo", ["build"], 1, string.Empty, "boom");

        var error = normalizer.NormalizeWorkflowFailure(
            "check",
            new DotnetCommandFailedException("build", result));

        error.Kind.Should().Be(CliErrorKind.BackendFailure);
        error.Code.Should().Be("CLI.BACKEND_EXEC_FAILURE");
        error.Details.Should().ContainKey("step").WhoseValue.Should().Be("build");
        error.Details.Should().ContainKey("exit_code").WhoseValue.Should().Be("1");
    }

    [Fact]
    public void NormalizeLintFailure_ReturnsBackendFailureForTool()
    {
        var normalizer = new BackendJsonNormalizer();

        var error = normalizer.NormalizeLintFailure(
            new ToolId("demagic"),
            new InvalidOperationException("boom"));

        error.Kind.Should().Be(CliErrorKind.BackendFailure);
        error.Code.Should().Be("CLI.BACKEND_EXEC_FAILURE");
        error.Details.Should().ContainKey("tool").WhoseValue.Should().Be("demagic");
        error.Details.Should().ContainKey("exception_type").WhoseValue.Should().Be(typeof(InvalidOperationException).FullName);
    }
}
