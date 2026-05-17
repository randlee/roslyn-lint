namespace Roslyn.Lint.Commands;

using System.Collections.Immutable;
using System.ComponentModel;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Formatting;
using Roslyn.Lint.Operations;
using Roslyn.Lint.Serialization;
using Spectre.Console;
using Spectre.Console.Cli;

/// <summary>
/// Command to lint C# files for code quality issues.
/// </summary>
public sealed class LintCommand : AsyncCommand<LintCommand.Settings>
{
    private static readonly ICommandOperation<LintRequest, LintResult> LintOperation =
        new LintOperation(new LintWorkspaceAdapter());

    private static readonly IJsonEnvelopeWriter JsonEnvelopeWriter = new JsonEnvelopeWriter();

    /// <summary>
    /// Settings for the lint command.
    /// </summary>
    public sealed class Settings : CommandSettings
    {
        /// <summary>
        /// Gets the path to a .cs file or directory to analyze.
        /// </summary>
        [CommandArgument(0, "<path>")]
        [Description("Path to a .cs file or directory to analyze")]
        public string Path { get; init; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether to emit the machine JSON envelope.
        /// </summary>
        [CommandOption("--json")]
        [Description("Emit the stable JSON envelope output")]
        [DefaultValue(false)]
        public bool Json { get; init; }

        /// <summary>
        /// Gets glob patterns to include (repeatable).
        /// </summary>
        [CommandOption("--include <pattern>")]
        [Description("Include files matching glob pattern (repeatable, e.g., \"*.cs\")")]
        public string[]? IncludePatterns { get; init; }

        /// <summary>
        /// Gets glob patterns to exclude (repeatable).
        /// </summary>
        [CommandOption("--exclude <pattern>")]
        [Description("Exclude files matching glob pattern (repeatable, e.g., \"**/obj/**\")")]
        public string[]? ExcludePatterns { get; init; }

        /// <summary>
        /// Gets a value indicating whether to disable colored output.
        /// </summary>
        [CommandOption("--no-color")]
        [Description("Disable colored output")]
        [DefaultValue(false)]
        public bool NoColor { get; init; }

        /// <inheritdoc/>
        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Path))
                return ValidationResult.Error("Path is required");

            return ValidationResult.Success();
        }
    }

    /// <inheritdoc/>
    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var request = new LintRequest(
            settings.Path,
            settings.Json,
            settings.IncludePatterns is null ? ImmutableArray<string>.Empty : [.. settings.IncludePatterns],
            settings.ExcludePatterns is null ? ImmutableArray<string>.Empty : [.. settings.ExcludePatterns],
            settings.NoColor);

        var envelope = await LintOperation.ExecuteAsync(request, cancellationToken);

        if (settings.Json)
        {
            JsonEnvelopeWriter.Write(envelope);
        }
        else
        {
            var formatter = new LintHumanOutputFormatter(settings.NoColor);
            if (envelope.Success && envelope.Result is not null)
            {
                formatter.WriteSuccess(envelope.Result, envelope.Warnings);
            }
            else
            {
                formatter.WriteFailure(
                    envelope.Error ?? new CliError(CliErrorKind.Internal, "CLI.UNKNOWN", "Unknown CLI failure"),
                    envelope.Warnings);
            }
        }

        if (!envelope.Success)
            return 1;

        return envelope.Result?.Issues.Any(issue => string.Equals(issue.Severity, "error", StringComparison.OrdinalIgnoreCase)) == true
            ? 1
            : 0;
    }
}
