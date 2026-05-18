namespace Roslyn.Lint.Serialization;

using System.Text.Json.Serialization;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Contracts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
[JsonSerializable(typeof(CliEnvelope<object>))]
[JsonSerializable(typeof(CliEnvelope<LintToolResult>))]
[JsonSerializable(typeof(CliEnvelope<LintProfileResult>))]
[JsonSerializable(typeof(CliEnvelope<CheckResult>))]
[JsonSerializable(typeof(CliEnvelope<ClippyResult>))]
[JsonSerializable(typeof(CliEnvelope<CiResult>))]
[JsonSerializable(typeof(CliEnvelope<VersionResult>))]
[JsonSerializable(typeof(CliEnvelope<ViewResult>))]
[JsonSerializable(typeof(CliError))]
[JsonSerializable(typeof(CliDiagnostic))]
[JsonSerializable(typeof(CheckRequest))]
[JsonSerializable(typeof(CheckResult))]
[JsonSerializable(typeof(CiRequest))]
[JsonSerializable(typeof(CiResult))]
[JsonSerializable(typeof(ClippyRequest))]
[JsonSerializable(typeof(ClippyResult))]
[JsonSerializable(typeof(LintFinding))]
[JsonSerializable(typeof(LintProfileResult))]
[JsonSerializable(typeof(LintToolRequest))]
[JsonSerializable(typeof(LintToolResult))]
[JsonSerializable(typeof(VersionResult))]
[JsonSerializable(typeof(ViewRequest))]
[JsonSerializable(typeof(ViewResult))]
[JsonSerializable(typeof(ViewToolResult))]
[JsonSerializable(typeof(WorkflowStepResult))]
public partial class RoslynLintJsonContext : JsonSerializerContext
{
}
