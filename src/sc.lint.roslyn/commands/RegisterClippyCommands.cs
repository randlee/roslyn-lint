namespace sc.lint.roslyn.commands;

using System.CommandLine;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.formatting;

internal static class RegisterClippyCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command(ScLintRoslynConstants.Commands.Clippy, "Run the stricter analyzer and formatting gate.");
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
                var result = await context.ClippyOperation.ExecuteAsync(
                    new ClippyRequest(targetPath, configuration),
                    cancellationToken);
                return await context.WriteSuccessAsync(
                    ScLintRoslynConstants.Commands.Clippy,
                    result,
                    new ClippyHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    ScLintRoslynConstants.Commands.Clippy,
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure(
                        ScLintRoslynConstants.Commands.Clippy,
                        exception),
                    outputMode,
                    cancellationToken);
            }
        });

        rootCommand.Subcommands.Add(command);
    }
}
