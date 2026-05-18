namespace Roslyn.Lint.Contracts;

public sealed record CheckRequest(string TargetPath, string Configuration);
