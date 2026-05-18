namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Formatting;

internal static class RegisterViewCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var viewCommand = new Command("view", "Inspect roslyn-lint metadata surfaces.");
        var toolsCommand = new Command("tools", "List registered lint tools.");
        var rulesCommand = new Command("rules", "Inspect rule metadata.");

        viewCommand.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "view",
                CreateUsageError(
                    "view requires a supported target such as 'tools' or 'rules'.",
                    new Dictionary<string, string?> { ["command"] = "view" }),
                context.GetOutputMode(parseResult),
                cancellationToken));

        toolsCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            try
            {
                var result = await context.ViewOperation.ExecuteAsync(
                    new ViewRequest("tools"),
                    cancellationToken);

                return await context.WriteSuccessAsync(
                    "view.tools",
                    result,
                    new ViewToolsHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    "view.tools",
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure("view.tools", exception),
                    outputMode,
                    cancellationToken);
            }
        });

        rulesCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);

            try
            {
                var result = await context.ViewOperation.ExecuteAsync(
                    new ViewRequest("rules"),
                    cancellationToken);

                return await context.WriteSuccessAsync(
                    "view.rules",
                    result,
                    new ViewRulesHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    "view.rules",
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure("view.rules", exception),
                    outputMode,
                    cancellationToken);
            }
        });

        viewCommand.Subcommands.Add(toolsCommand);
        viewCommand.Subcommands.Add(rulesCommand);
        rootCommand.Subcommands.Add(viewCommand);
    }

    private static CliError CreateUsageError(string message, IReadOnlyDictionary<string, string?> details)
        => new(
            CliErrorKind.Usage,
            "CLI.USAGE_ERROR",
            message,
            details,
            "Run the command with a supported subcommand or use --help.");
}
