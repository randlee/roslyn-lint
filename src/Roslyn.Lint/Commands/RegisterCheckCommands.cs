namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Contracts;

internal static class RegisterCheckCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("check", "Run the planned check workflow.");

        command.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "check",
                new CliError(
                    CliErrorKind.Capability,
                    "CLI.CAPABILITY_ERROR",
                    "check is planned for sprint A7.",
                    new Dictionary<string, string?> { ["planned_sprint"] = "A7" },
                    "Use 'version' or 'view tools' until the workflow lands."),
                context.GetOutputMode(parseResult),
                cancellationToken));

        rootCommand.Subcommands.Add(command);
    }
}
