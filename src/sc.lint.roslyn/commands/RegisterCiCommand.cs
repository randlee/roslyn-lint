namespace sc.lint.roslyn.commands;

using System.CommandLine;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.formatting;

internal static class RegisterCiCommand
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("ci", "Run lint plus tests.");
        var pathOption = new Option<string>("--path")
        {
            DefaultValueFactory = _ => ".",
            Description = "Directory that contains the sc-lint-roslyn solution.",
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
                var result = await context.CiOperation.ExecuteAsync(
                    new CiRequest(targetPath, configuration),
                    cancellationToken);
                return await context.WriteSuccessAsync(
                    "ci",
                    result,
                    new CiHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    "ci",
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure("ci", exception),
                    outputMode,
                    cancellationToken);
            }
        });

        rootCommand.Subcommands.Add(command);
    }
}
