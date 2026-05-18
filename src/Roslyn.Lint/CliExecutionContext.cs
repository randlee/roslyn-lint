namespace Roslyn.Lint;

using System.CommandLine;
using System.CommandLine.Parsing;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.CommandModel;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Formatting;
using Roslyn.Lint.Serialization;

internal sealed class CliExecutionContext
{
    public CliExecutionContext(
        TextWriter output,
        TextWriter error,
        Option<bool> jsonOption,
        IReadOnlyList<ILintToolModule> toolModules,
        IJsonEnvelopeWriter jsonEnvelopeWriter,
        string version)
    {
        Output = output;
        Error = error;
        JsonOption = jsonOption;
        ToolModules = toolModules;
        JsonEnvelopeWriter = jsonEnvelopeWriter;
        Version = version;
    }

    public TextWriter Output { get; }

    public TextWriter Error { get; }

    public Option<bool> JsonOption { get; }

    public IReadOnlyList<ILintToolModule> ToolModules { get; }

    public IJsonEnvelopeWriter JsonEnvelopeWriter { get; }

    public string Version { get; }

    public OutputMode GetOutputMode(ParseResult parseResult)
        => parseResult.GetValue(JsonOption) ? OutputMode.Json : OutputMode.Text;

    public async Task<int> WriteSuccessAsync<T>(
        string command,
        T data,
        IHumanOutputFormatter<T> formatter,
        OutputMode outputMode,
        CancellationToken cancellationToken)
    {
        if (outputMode == OutputMode.Json)
        {
            await JsonEnvelopeWriter.WriteAsync(
                Output,
                CliEnvelope<T>.Success(command, data),
                cancellationToken);
            return 0;
        }

        await formatter.WriteAsync(Output, data, cancellationToken);
        return 0;
    }

    public async Task<int> WriteFailureAsync(
        string command,
        CliError error,
        OutputMode outputMode,
        CancellationToken cancellationToken)
    {
        if (outputMode == OutputMode.Json)
        {
            await JsonEnvelopeWriter.WriteAsync(
                Output,
                CliEnvelope<object>.Failure(command, error),
                cancellationToken);
            return 1;
        }

        await Error.WriteLineAsync($"{error.Code}: {error.Message}".AsMemory(), cancellationToken);

        if (!string.IsNullOrWhiteSpace(error.SuggestedAction))
            await Error.WriteLineAsync($"Suggested action: {error.SuggestedAction}".AsMemory(), cancellationToken);

        return 1;
    }
}
