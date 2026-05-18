namespace Roslyn.Lint.Contracts;

using System.Text.Json.Serialization;

public sealed record CliError
{
    public CliError(
        CliErrorKind kind,
        string code,
        string message,
        IReadOnlyDictionary<string, string?>? details = null,
        string? suggestedAction = null)
    {
        Kind = kind;
        Code = code;
        Message = message;
        Details = details ?? new Dictionary<string, string?>();
        SuggestedAction = suggestedAction;
    }

    public CliErrorKind Kind { get; }

    public string Code { get; }

    public string Message { get; }

    public IReadOnlyDictionary<string, string?> Details { get; }

    [JsonPropertyName("suggested_action")]
    public string? SuggestedAction { get; }
}
