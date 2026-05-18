namespace Roslyn.Lint.Contracts;

public sealed record ViewRequest(string Target, string? ToolId = null);
