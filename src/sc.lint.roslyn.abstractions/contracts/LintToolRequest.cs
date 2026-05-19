namespace sc.lint.roslyn.abstractions.contracts;

using sc.lint.roslyn.abstractions;

public sealed record LintToolRequest(ToolId Tool, string TargetPath);
