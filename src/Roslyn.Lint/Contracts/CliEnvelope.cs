namespace Roslyn.Lint.Contracts;

using System.Collections.Immutable;

public sealed record CliEnvelope<TResult>(
    bool Success,
    string Operation,
    TResult? Result,
    CliError? Error,
    ImmutableArray<CliWarning> Warnings)
{
    public static CliEnvelope<TResult> Ok(string operation, TResult result, ImmutableArray<CliWarning> warnings = default)
        => new(true, operation, result, null, warnings.IsDefault ? ImmutableArray<CliWarning>.Empty : warnings);

    public static CliEnvelope<TResult> Fail(string operation, CliError error, ImmutableArray<CliWarning> warnings = default)
        => new(false, operation, default, error, warnings.IsDefault ? ImmutableArray<CliWarning>.Empty : warnings);
}
