namespace Roslyn.Lint.Abstractions.Contracts;

public sealed record ViewRequest(string Target, string? ToolId = null);
