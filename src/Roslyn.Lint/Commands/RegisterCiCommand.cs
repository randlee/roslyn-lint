namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.Abstractions.Contracts;

internal static class RegisterCiCommand
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("ci", "Run the planned CI workflow.");

        command.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "ci",
                new CliError(
                    CliErrorKind.Capability,
                    "CLI.CAPABILITY_ERROR",
                    "ci is planned for sprint A7.",
                    new Dictionary<string, string?> { ["planned_sprint"] = "A7" },
                    "Use the repo build and test commands directly until A7 lands."),
                context.GetOutputMode(parseResult),
                cancellationToken));

        rootCommand.Subcommands.Add(command);
    }
}
