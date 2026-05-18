namespace Roslyn.Lint.Dispatch;

using System.Text.Json.Serialization.Metadata;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;

public interface IBackendJsonNormalizer
{
    CliError NormalizeLintFailure(ToolId toolId, Exception exception);

    CliError NormalizeWorkflowFailure(string commandId, Exception exception);

    DelegatedBackendNormalizationResult<T> NormalizeDelegatedResult<T>(
        string commandId,
        BackendProcessResult result,
        JsonTypeInfo<T> dataTypeInfo);
}
