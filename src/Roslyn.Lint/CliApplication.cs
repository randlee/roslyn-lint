namespace Roslyn.Lint;

using System.CommandLine;
using Roslyn.Lint.Abstractions;
using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.Backends;
using Roslyn.Lint.Commands;
using Roslyn.Lint.Dispatch;
using Roslyn.Lint.Operations;
using Roslyn.Lint.Serialization;

public sealed class CliApplication
{
    private readonly IReadOnlyList<ILintToolModule> toolModules;
    private readonly ILintToolOperation lintToolOperation;
    private readonly BackendJsonNormalizer backendJsonNormalizer;
    private readonly IJsonEnvelopeWriter jsonEnvelopeWriter;

    public CliApplication(
        IReadOnlyList<ILintToolModule>? toolModules = null,
        IJsonEnvelopeWriter? jsonEnvelopeWriter = null)
    {
        this.toolModules = toolModules ?? [new RoslynDeMagicToolModule()];
        var dispatcher = new BackendToolDispatcher(this.toolModules);
        lintToolOperation = new RunLintToolOperation(dispatcher);
        backendJsonNormalizer = new BackendJsonNormalizer();
        this.jsonEnvelopeWriter = jsonEnvelopeWriter ?? new JsonEnvelopeWriter();
    }

    public static Task<int> RunAsync(
        string[] args,
        TextWriter output,
        TextWriter error,
        CancellationToken cancellationToken)
        => new CliApplication().RunInternalAsync(args, output, error, cancellationToken);

    public async Task<int> RunInternalAsync(
        string[] args,
        TextWriter output,
        TextWriter error,
        CancellationToken cancellationToken)
    {
        var jsonOption = new Option<bool>("--json")
        {
            Description = "Emit machine-readable JSON output.",
            Recursive = true,
        };
        var context = new CliExecutionContext(
            output,
            error,
            jsonOption,
            toolModules,
            lintToolOperation,
            backendJsonNormalizer,
            jsonEnvelopeWriter,
            typeof(CliApplication).Assembly.GetName().Version?.ToString() ?? "0.0.0");

        var rootCommand = BuildRootCommand(context);
        var parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count > 0)
        {
            var outputMode = context.GetOutputMode(parseResult);
            var errorPayload = new CliError(
                CliErrorKind.Usage,
                "CLI.USAGE_ERROR",
                parseResult.Errors[0].Message,
                new Dictionary<string, string?>
                {
                    ["error_count"] = parseResult.Errors.Count.ToString(),
                },
                "Fix the command arguments or run with --help.");

            return await context.WriteFailureAsync("root", errorPayload, outputMode, cancellationToken);
        }

        return await parseResult.InvokeAsync(cancellationToken: cancellationToken);
    }

    internal RootCommand BuildRootCommand(CliExecutionContext context)
    {
        var rootCommand = new RootCommand("AI-first orchestration CLI for the roslyn-lint suite.");
        rootCommand.Options.Add(context.JsonOption);

        rootCommand.SetAction((parseResult, cancellationToken) =>
            context.WriteFailureAsync(
                "root",
                new CliError(
                    CliErrorKind.Usage,
                    "CLI.USAGE_ERROR",
                    "A command is required.",
                    new Dictionary<string, string?> { ["command"] = "root" },
                    "Run a supported command such as 'version' or 'view tools'."),
                context.GetOutputMode(parseResult),
                cancellationToken));

        RegisterLintCommands.AddTo(rootCommand, context);
        RegisterViewCommands.AddTo(rootCommand, context);
        RegisterCheckCommands.AddTo(rootCommand, context);
        RegisterClippyCommands.AddTo(rootCommand, context);
        RegisterCiCommand.AddTo(rootCommand, context);
        RegisterVersionCommand.AddTo(rootCommand, context);

        return rootCommand;
    }
}
