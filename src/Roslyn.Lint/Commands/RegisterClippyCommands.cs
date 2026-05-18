namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Abstractions.Contracts;

internal static class RegisterClippyCommands
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("clippy", "Run the planned analyzer gate workflow.");

        command.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "clippy",
                new CliError(
                    CliErrorKind.Capability,
                    "CLI.CAPABILITY_ERROR",
                    "clippy is planned for sprint A7.",
                    new Dictionary<string, string?> { ["planned_sprint"] = "A7" },
                    "Use 'view tools' to inspect the currently available tool surface."),
                context.GetOutputMode(parseResult),
                cancellationToken));

        rootCommand.Subcommands.Add(command);
    }
}
