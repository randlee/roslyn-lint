namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Contracts;

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

        lintCommand.Subcommands.Add(CreatePlaceholderCommand("demagic", "lint.demagic", "A6", context));
        lintCommand.Subcommands.Add(CreatePlaceholderCommand("fast", "lint.fast", "A6", context));
        lintCommand.Subcommands.Add(CreatePlaceholderCommand("full", "lint.full", "A7", context));
        lintCommand.Subcommands.Add(CreatePlaceholderCommand("ci", "lint.ci", "A7", context));

        rootCommand.Subcommands.Add(lintCommand);
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
