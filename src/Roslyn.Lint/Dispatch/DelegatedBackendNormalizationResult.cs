namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions.Contracts;

public sealed record DelegatedBackendNormalizationResult<T>(bool IsSuccess, T? Data, CliError? Error)
{
    public static DelegatedBackendNormalizationResult<T> Success(T data) => new(true, data, null);

    public static DelegatedBackendNormalizationResult<T> Failure(CliError error) => new(false, default, error);
}
