namespace sc.lint.roslyn.commands;

using System.CommandLine;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.commandmodel;
using sc.lint.roslyn.contracts;
using sc.lint.roslyn.formatting;

internal static class RegisterVersionCommand
{
    public static void AddTo(RootCommand rootCommand, CliExecutionContext context)
    {
        var command = new Command(ScLintRoslynConstants.Commands.Version, "Show the sc-lint-roslyn CLI version.");

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            var result = new VersionResult(ScLintRoslynConstants.Suite.CliName, context.Version);
            return await context.WriteSuccessAsync(
                ScLintRoslynConstants.Commands.Version,
                result,
                new VersionHumanOutputFormatter(),
                outputMode,
                cancellationToken);
        });

        rootCommand.Subcommands.Add(command);
    }
}
