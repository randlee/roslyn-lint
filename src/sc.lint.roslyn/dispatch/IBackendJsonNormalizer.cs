namespace sc.lint.roslyn.dispatch;

using System.Text.Json.Serialization.Metadata;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public interface IBackendJsonNormalizer
{
    CliError NormalizeLintFailure(ToolId toolId, Exception exception);

    CliError NormalizeWorkflowFailure(string commandId, Exception exception);

    DelegatedBackendNormalizationResult<T> NormalizeDelegatedResult<T>(
        string commandId,
        BackendProcessResult result,
        JsonTypeInfo<T> dataTypeInfo);
}
