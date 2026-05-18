namespace Roslyn.Lint.Dispatch;

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Serialization;

public sealed class BackendJsonNormalizer : IBackendJsonNormalizer
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
            BackendProcessUnavailableException processUnavailable => new CliError(
                CliErrorKind.Capability,
                "CLI.CAPABILITY_ERROR",
                $"The backend process '{processUnavailable.FileName}' is not available.",
                new Dictionary<string, string?>
                {
                    ["command"] = commandId,
                    ["tool"] = processUnavailable.FileName,
                },
                "Install the required backend executable and re-run the command."),
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

    public DelegatedBackendNormalizationResult<T> NormalizeDelegatedResult<T>(
        string commandId,
        BackendProcessResult result,
        JsonTypeInfo<T> dataTypeInfo)
    {
        if (TryParseRootObject(result.StandardOutput, out var root, out var parseError))
        {
            var parsedRoot = root ?? throw new InvalidOperationException("Delegated backend JSON parse returned no document.");
            using (parsedRoot)
            {
                var rootElement = parsedRoot.RootElement;

                if (TryReadCommand(rootElement, out var payloadCommand) && !string.Equals(payloadCommand, commandId, StringComparison.Ordinal))
                {
                    return DelegatedBackendNormalizationResult<T>.Failure(CreateProtocolError(
                        commandId,
                        "Delegated backend returned a mismatched command identifier.",
                        new Dictionary<string, string?>
                        {
                            ["expected_command"] = commandId,
                            ["actual_command"] = payloadCommand,
                        }));
                }

                if (!TryReadOk(rootElement, out var ok))
                {
                    return DelegatedBackendNormalizationResult<T>.Failure(CreateResultError(commandId, result, "Delegated backend output did not contain a valid 'ok' flag."));
                }

                if (ok)
                {
                    if (result.ExitCode != 0)
                    {
                        return DelegatedBackendNormalizationResult<T>.Failure(CreateExecutionError(
                            commandId,
                            result,
                            "Delegated backend exited non-zero while reporting success."));
                    }

                    if (!rootElement.TryGetProperty("data", out var dataElement))
                    {
                        return DelegatedBackendNormalizationResult<T>.Failure(CreateProtocolError(
                            commandId,
                            "Delegated backend success payload omitted 'data'.",
                            new Dictionary<string, string?> { ["command"] = commandId }));
                    }

                    try
                    {
                        var data = dataElement.Deserialize(dataTypeInfo);
                        return data is null
                            ? DelegatedBackendNormalizationResult<T>.Failure(CreateProtocolError(
                                commandId,
                                "Delegated backend success payload could not be deserialized.",
                                new Dictionary<string, string?> { ["command"] = commandId }))
                            : DelegatedBackendNormalizationResult<T>.Success(data);
                    }
                    catch (JsonException)
                    {
                        return DelegatedBackendNormalizationResult<T>.Failure(CreateProtocolError(
                            commandId,
                            "Delegated backend success payload was malformed.",
                            new Dictionary<string, string?> { ["command"] = commandId }));
                    }
                }

                if (result.ExitCode == 0)
                {
                    return DelegatedBackendNormalizationResult<T>.Failure(CreateProtocolError(
                        commandId,
                        "Delegated backend reported failure with a zero exit code.",
                        new Dictionary<string, string?> { ["command"] = commandId }));
                }

                if (!rootElement.TryGetProperty("error", out var errorElement))
                {
                    return DelegatedBackendNormalizationResult<T>.Failure(CreateExecutionError(
                        commandId,
                        result,
                        "Delegated backend failed without an 'error' payload."));
                }

                try
                {
                    var error = errorElement.Deserialize(RoslynLintJsonContext.Default.CliError);
                    return error is null
                        ? DelegatedBackendNormalizationResult<T>.Failure(CreateExecutionError(
                            commandId,
                            result,
                            "Delegated backend failure payload could not be deserialized."))
                        : DelegatedBackendNormalizationResult<T>.Failure(error);
                }
                catch (JsonException)
                {
                    return DelegatedBackendNormalizationResult<T>.Failure(CreateExecutionError(
                        commandId,
                        result,
                        "Delegated backend failure payload was malformed."));
                }
            }
        }

        return DelegatedBackendNormalizationResult<T>.Failure(
            result.ExitCode == 0
                ? CreateProtocolError(
                    commandId,
                    "Delegated backend emitted malformed JSON.",
                    new Dictionary<string, string?>
                    {
                        ["command"] = commandId,
                        ["exception_type"] = parseError?.GetType().FullName,
                    })
                : CreateExecutionError(
                    commandId,
                    result,
                    "Delegated backend exited non-zero without a valid machine-readable payload."));
    }

    private static bool TryParseRootObject(string json, out JsonDocument? document, out Exception? exception)
    {
        try
        {
            document = JsonDocument.Parse(json);
            exception = null;
            return document.RootElement.ValueKind == JsonValueKind.Object;
        }
        catch (Exception parseException) when (parseException is JsonException or ArgumentException)
        {
            document = null;
            exception = parseException;
            return false;
        }
    }

    private static bool TryReadOk(JsonElement root, out bool ok)
    {
        if (root.TryGetProperty("ok", out var okElement) && (okElement.ValueKind == JsonValueKind.True || okElement.ValueKind == JsonValueKind.False))
        {
            ok = okElement.GetBoolean();
            return true;
        }

        ok = false;
        return false;
    }

    private static bool TryReadCommand(JsonElement root, out string command)
    {
        if (root.TryGetProperty("command", out var commandElement) && commandElement.ValueKind == JsonValueKind.String)
        {
            command = commandElement.GetString() ?? string.Empty;
            return true;
        }

        command = string.Empty;
        return false;
    }

    private static CliError CreateProtocolError(string commandId, string message, IReadOnlyDictionary<string, string?> details)
        => new(
            CliErrorKind.BackendProtocol,
            "CLI.BACKEND_PROTOCOL_ERROR",
            message,
            details,
            "Inspect the delegated backend machine output and re-run with a matching workspace revision.");

    private static CliError CreateExecutionError(string commandId, BackendProcessResult result, string message)
        => new(
            CliErrorKind.BackendFailure,
            "CLI.BACKEND_EXEC_FAILURE",
            message,
            new Dictionary<string, string?>
            {
                ["command"] = commandId,
                ["tool"] = result.FileName,
                ["arguments"] = result.ArgumentsDisplay,
                ["exit_code"] = result.ExitCode.ToString(),
            },
            "Inspect the delegated backend stderr/stdout details and re-run after addressing the failure.");

    private static CliError CreateResultError(string commandId, BackendProcessResult result, string message)
        => result.ExitCode == 0
            ? CreateProtocolError(commandId, message, new Dictionary<string, string?> { ["command"] = commandId })
            : CreateExecutionError(commandId, result, message);
}
