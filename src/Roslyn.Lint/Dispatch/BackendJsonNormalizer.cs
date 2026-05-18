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

    public CliError NormalizeWorkflowFailure(string commandId, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception switch
        {
            Backends.DotnetToolUnavailableException => new CliError(
                CliErrorKind.Capability,
                "CLI.CAPABILITY_ERROR",
                "The dotnet CLI is not available.",
                new Dictionary<string, string?>
                {
                    ["command"] = commandId,
                    ["tool"] = "dotnet",
                },
                "Install a supported .NET SDK and re-run the workflow."),
            Backends.CiLintGateFailedException lintGateFailure => new CliError(
                CliErrorKind.BackendFailure,
                "CLI.CI_LINT_FAILURE",
                "The ci lint gate reported findings.",
                new Dictionary<string, string?>
                {
                    ["command"] = commandId,
                    ["profile"] = lintGateFailure.Result.Profile,
                    ["finding_count"] = lintGateFailure.Result.FindingCount.ToString(),
                },
                "Resolve the reported lint findings and re-run ci."),
            Backends.DotnetCommandFailedException dotnetFailure => new CliError(
                CliErrorKind.BackendFailure,
                "CLI.BACKEND_EXEC_FAILURE",
                $"dotnet step '{dotnetFailure.StepName}' failed during {commandId}.",
                new Dictionary<string, string?>
                {
                    ["command"] = commandId,
                    ["step"] = dotnetFailure.StepName,
                    ["tool"] = "dotnet",
                    ["arguments"] = dotnetFailure.Result.ArgumentsDisplay,
                    ["exit_code"] = dotnetFailure.Result.ExitCode.ToString(),
                },
                "Inspect the failed workflow step and re-run after addressing the build or test issue."),
            _ => new CliError(
                CliErrorKind.Internal,
                "CLI.INTERNAL_ERROR",
                $"{commandId} failed unexpectedly.",
                new Dictionary<string, string?>
                {
                    ["command"] = commandId,
                    ["exception_type"] = exception.GetType().FullName,
                },
                "Inspect the exception details and re-run the command."),
        };
    }
}
