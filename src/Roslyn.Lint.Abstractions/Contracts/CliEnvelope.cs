namespace Roslyn.Lint.Abstractions.Contracts;

using System.Text.Json.Serialization;

public sealed record CliEnvelope<T>
{
    private CliEnvelope(
        bool ok,
        string command,
        T? data,
        CliError? error,
        IReadOnlyList<CliDiagnostic>? diagnostics)
    {
        Ok = ok;
        Command = command;
        Data = data;
        Error = error;
        Diagnostics = diagnostics is { Count: > 0 } ? diagnostics : null;
    }

    public bool Ok { get; }

    public string Command { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CliError? Error { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CliDiagnostic>? Diagnostics { get; }

    public static CliEnvelope<T> Success(string command, T data, IReadOnlyList<CliDiagnostic>? diagnostics = null)
        => new(true, command, data, null, diagnostics);

    public static CliEnvelope<T> Failure(string command, CliError error, IReadOnlyList<CliDiagnostic>? diagnostics = null)
        => new(false, command, default, error, diagnostics);
}
