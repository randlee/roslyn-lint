namespace Roslyn.Lint.Contracts;

public sealed record ViewResult(string Target, IReadOnlyList<ViewToolResult> Tools);

public sealed record ViewToolResult(
    string Id,
    string DisplayName,
    string Description,
    string PackageName,
    IReadOnlyList<string> CommandFamilies,
    IReadOnlyList<string> Capabilities);
