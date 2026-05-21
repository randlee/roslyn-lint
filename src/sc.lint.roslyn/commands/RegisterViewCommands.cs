namespace sc.lint.roslyn.commands;

using System.CommandLine;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;
using sc.lint.roslyn.formatting;

internal static class RegisterViewCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var viewCommand = new Command("view", "Inspect sc-lint-roslyn metadata surfaces.");
        var toolsCommand = new Command("tools", "List registered lint tools.");
        var rulesCommand = new Command("rules", "Inspect rule metadata.");

        viewCommand.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "view",
                CreateUsageError(
                    $"view requires a supported target such as '{ScLintRoslynConstants.Commands.ToolsName}' or '{ScLintRoslynConstants.Commands.RulesName}'.",
                    new Dictionary<string, string?> { ["command"] = "view" }),
                context.GetOutputMode(parseResult),
                cancellationToken));

        toolsCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            try
            {
                var result = await context.ViewOperation.ExecuteAsync(
                    new ViewRequest(ScLintRoslynConstants.Commands.ToolsName),
                    cancellationToken);

                return await context.WriteSuccessAsync(
                    ScLintRoslynConstants.Commands.ViewTools,
                    result,
                    new ViewToolsHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    ScLintRoslynConstants.Commands.ViewTools,
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure(ScLintRoslynConstants.Commands.ViewTools, exception),
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
                    new ViewRequest(ScLintRoslynConstants.Commands.RulesName),
                    cancellationToken);

                return await context.WriteSuccessAsync(
                    ScLintRoslynConstants.Commands.ViewRules,
                    result,
                    new ViewRulesHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    ScLintRoslynConstants.Commands.ViewRules,
                    context.BackendJsonNormalizer.NormalizeWorkflowFailure(ScLintRoslynConstants.Commands.ViewRules, exception),
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
