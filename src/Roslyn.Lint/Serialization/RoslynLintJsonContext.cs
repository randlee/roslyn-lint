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
[JsonSerializable(typeof(CliEnvelope<VersionResult>))]
[JsonSerializable(typeof(CliEnvelope<ViewResult>))]
[JsonSerializable(typeof(CliError))]
[JsonSerializable(typeof(CliDiagnostic))]
[JsonSerializable(typeof(LintFinding))]
[JsonSerializable(typeof(LintToolRequest))]
[JsonSerializable(typeof(LintToolResult))]
[JsonSerializable(typeof(VersionResult))]
[JsonSerializable(typeof(ViewRequest))]
[JsonSerializable(typeof(ViewResult))]
[JsonSerializable(typeof(ViewToolResult))]
public partial class RoslynLintJsonContext : JsonSerializerContext
{
}
