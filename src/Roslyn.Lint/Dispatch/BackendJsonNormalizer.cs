namespace Roslyn.Lint.Dispatch;

using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;

public sealed class BackendJsonNormalizer
{
    public CliError NormalizeLintFailure(ToolId toolId, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new CliError(
            CliErrorKind.BackendFailure,
            "CLI.BACKEND_EXEC_FAILURE",
            $"{toolId.Value} backend failed during lint execution.",
            new Dictionary<string, string?>
            {
                ["tool"] = toolId.Value,
                ["exception_type"] = exception.GetType().FullName,
            },
            "Inspect the backend exception details and re-run against a supported workspace revision.");
    }
}
