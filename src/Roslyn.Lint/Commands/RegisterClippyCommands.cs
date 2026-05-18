namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Formatting;

internal static class RegisterClippyCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("clippy", "Run the stricter analyzer and formatting gate.");
        var pathOption = new Option<string>("--path")
        {
            DefaultValueFactory = _ => ".",
            Description = "Directory that contains the roslyn-lint solution.",
        };
        var configurationOption = new Option<string>("--configuration")
        {
            DefaultValueFactory = _ => "Release",
            Description = "Build configuration to use.",
        };

        command.Options.Add(pathOption);
        command.Options.Add(configurationOption);
        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            var targetPath = Path.GetFullPath(parseResult.GetValue(pathOption) ?? ".");
            var configuration = parseResult.GetValue(configurationOption) ?? "Release";

            try
            {
                var result = await context.ClippyOperation.ExecuteAsync(
                    new ClippyRequest(targetPath, configuration),
                    cancellationToken);
                return await context.WriteSuccessAsync(
                    "clippy",
                    result,
                    new ClippyHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    "clippy",
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure("clippy", exception),
                    outputMode,
                    cancellationToken);
            }
        });

        rootCommand.Subcommands.Add(command);
    }
}
