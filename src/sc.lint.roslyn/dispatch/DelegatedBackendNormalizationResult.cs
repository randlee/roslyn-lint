namespace sc.lint.roslyn.dispatch;

using sc.lint.roslyn.abstractions.contracts;

public sealed record DelegatedBackendNormalizationResult<T>(bool IsSuccess, T? Data, CliError? Error)
{
    public static DelegatedBackendNormalizationResult<T> Success(T data) => new(true, data, null);

    public static DelegatedBackendNormalizationResult<T> Failure(CliError error) => new(false, default, error);
}
