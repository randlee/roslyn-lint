namespace sc.lint.roslyn.abstractions.contracts;

public sealed record ViewRequest(string Target, string? ToolId = null);
