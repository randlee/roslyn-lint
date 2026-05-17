namespace Roslyn.Lint.Contracts;

public sealed record CliError(
    CliErrorKind Kind,
    string Code,
    string Message,
    IReadOnlyDictionary<string, string>? Details = null,
    string? SuggestedAction = null);
