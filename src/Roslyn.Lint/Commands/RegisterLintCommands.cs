namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Formatting;

internal static class RegisterLintCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var lintCommand = new Command("lint", "Run lint tools and lint profiles.");

        lintCommand.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "lint",
                CreateUsageError(
                    "lint requires a tool or profile such as 'demagic' or 'fast'.",
                    new Dictionary<string, string?> { ["command"] = "lint" }),
                context.GetOutputMode(parseResult),
                cancellationToken));

        lintCommand.Subcommands.Add(CreateLintCommand("demagic", "lint.demagic", new ToolId("demagic"), context));
        lintCommand.Subcommands.Add(CreateLintCommand("fast", "lint.fast", new ToolId("demagic"), context));
        lintCommand.Subcommands.Add(CreatePlaceholderCommand("full", "lint.full", "A7", context));
        lintCommand.Subcommands.Add(CreatePlaceholderCommand("ci", "lint.ci", "A7", context));

        rootCommand.Subcommands.Add(lintCommand);
    }

    private static Command CreateLintCommand(
        string name,
        string commandId,
        ToolId toolId,
        CliExecutionContext context)
    {
        var command = new Command(name);
        var pathOption = new Option<string>("--path")
        {
            DefaultValueFactory = _ => ".",
            Description = "File or directory to lint.",
        };
        command.Options.Add(pathOption);
        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            var rawPath = parseResult.GetValue(pathOption) ?? ".";
            var targetPath = Path.GetFullPath(rawPath);

            if (!Directory.Exists(targetPath) && !File.Exists(targetPath))
            {
                return await context.WriteFailureAsync(
                    commandId,
                    CreateUsageError(
                        $"Path '{rawPath}' does not exist.",
                        new Dictionary<string, string?> { ["path"] = rawPath }),
                    outputMode,
                    cancellationToken);
            }

            try
            {
                var result = await context.LintToolOperation.ExecuteAsync(
                    new LintToolRequest(toolId, targetPath),
                    cancellationToken);
                return await context.WriteSuccessAsync(
                    commandId,
                    result,
                    new LintToolHumanOutputFormatter(),
                    outputMode,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return await context.WriteFailureAsync(
                    commandId,
                    context.BackendJsonNormalizer.NormalizeLintFailure(toolId, exception),
                    outputMode,
                    cancellationToken);
            }
        });

        return command;
    }

    private static Command CreatePlaceholderCommand(
        string name,
        string commandId,
        string sprint,
        CliExecutionContext context)
    {
        var command = new Command(name);
        command.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                commandId,
                new CliError(
                    CliErrorKind.Capability,
                    "CLI.CAPABILITY_ERROR",
                    $"{commandId} is not implemented yet.",
                    new Dictionary<string, string?> { ["planned_sprint"] = sprint },
                    "Follow the sprint plan for the next implementation milestone."),
                context.GetOutputMode(parseResult),
                cancellationToken));

        return command;
    }

    private static CliError CreateUsageError(string message, IReadOnlyDictionary<string, string?> details)
        => new(
            CliErrorKind.Usage,
            "CLI.USAGE_ERROR",
            message,
            details,
            "Run the command with a supported tool or profile or use --help.");
}
