namespace sc.lint.roslyn.commands;

using System.CommandLine;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.formatting;

internal static class RegisterCheckCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command(ScLintRoslynConstants.Commands.Check, "Run the build and compile gate.");
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
                var result = await context.CheckOperation.ExecuteAsync(
                    new CheckRequest(targetPath, configuration),
                    cancellationToken);
                return await context.WriteSuccessAsync(
                    ScLintRoslynConstants.Commands.Check,
                    result,
                    new CheckHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    ScLintRoslynConstants.Commands.Check,
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure(
                        ScLintRoslynConstants.Commands.Check,
                        exception),
                    outputMode,
                    cancellationToken);
            }
        });

        rootCommand.Subcommands.Add(command);
    }
}
