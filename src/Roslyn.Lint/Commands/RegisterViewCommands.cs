namespace Roslyn.Lint.Commands;

using System.CommandLine;
using Roslyn.Lint.CommandModel;
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
                    "view requires a supported target such as 'tools'.",
                    new Dictionary<string, string?> { ["command"] = "view" }),
                context.GetOutputMode(parseResult),
                cancellationToken));

        toolsCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            var outputMode = context.GetOutputMode(parseResult);
            var result = new ViewResult(
                "tools",
                context.ToolModules
                    .Select(module => new ViewToolResult(
                        module.Descriptor.Id.Value,
                        module.Descriptor.DisplayName,
                        module.Descriptor.Description,
                        module.Descriptor.PackageName,
                        module.Descriptor.CommandFamilies,
                        module.Descriptor.Capabilities))
                    .ToArray());

            return await context.WriteSuccessAsync(
                "view.tools",
                result,
                new ViewToolsHumanOutputFormatter(),
                outputMode,
                cancellationToken);
        });

        rulesCommand.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "view.rules",
                CreateCapabilityError(
                    "view rules is planned for sprint A8.",
                    "A8",
                    "Use 'view tools' for the currently supported inspection surface."),
                context.GetOutputMode(parseResult),
                cancellationToken));

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

    private static CliError CreateCapabilityError(string message, string sprint, string suggestedAction)
        => new(
            CliErrorKind.Capability,
            "CLI.CAPABILITY_ERROR",
            message,
            new Dictionary<string, string?> { ["planned_sprint"] = sprint },
            suggestedAction);
}
