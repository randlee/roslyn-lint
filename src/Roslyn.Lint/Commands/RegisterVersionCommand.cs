namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.CommandModel;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Formatting;

internal static class RegisterVersionCommand
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command("version", "Show the roslyn-lint CLI version.");

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            var result = new VersionResult("roslyn-lint", context.Version);
            return await context.WriteSuccessAsync(
                "version",
                result,
                new VersionHumanOutputFormatter(),
                outputMode,
                cancellationToken);
        });

        rootCommand.Subcommands.Add(command);
    }
}
