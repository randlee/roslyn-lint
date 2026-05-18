namespace Roslyn.Lint.Contracts;

using Roslyn.Lint.Abstractions;

public sealed record LintToolRequest(ToolId Tool, string TargetPath);
