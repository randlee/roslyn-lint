namespace Roslyn.Lint.Commands;

using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.DeMagic.Analyzers;
using Spectre.Console;
using Spectre.Console.Cli;

/// <summary>
/// Command to lint C# files for code quality issues.
/// </summary>
public sealed class LintCommand : AsyncCommand<LintCommand.Settings>
{
    private static readonly ImmutableArray<DiagnosticAnalyzer> Analyzers =
    [
        new MagicNumberAnalyzer(),
        new DM002ForbiddenStringLiteralAnalyzer(),
    ];

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
        /// Gets the output format.
        /// </summary>
        [CommandOption("-f|--format <format>")]
        [Description("Output format: text or json [[default: text]]")]
        [DefaultValue("text")]
        public string Format { get; init; } = "text";

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

            var validFormats = new[] { "text", "json" };
            if (!validFormats.Contains(Format.ToLowerInvariant()))
                return ValidationResult.Error($"Invalid format: '{Format}'. Valid formats: text, json");

            return ValidationResult.Success();
        }
    }

    /// <inheritdoc/>
    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var files = ResolveFiles(settings.Path, settings.IncludePatterns, settings.ExcludePatterns);

        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No C# files found.[/]");
            return 0;
        }

        var allDiagnostics = new List<Diagnostic>();

        foreach (var file in files)
        {
            var source = await File.ReadAllTextAsync(file, cancellationToken);
            var tree = CSharpSyntaxTree.ParseText(source, path: file, cancellationToken: cancellationToken);

            var compilation = CSharpCompilation.Create(
                assemblyName: System.IO.Path.GetFileNameWithoutExtension(file),
                syntaxTrees: [tree],
                references: [],
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var compilationWithAnalyzers = compilation.WithAnalyzers(Analyzers);
            var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken);
            allDiagnostics.AddRange(diagnostics);
        }

        if (settings.Format.Equals("json", StringComparison.OrdinalIgnoreCase))
        {
            WriteJsonOutput(allDiagnostics);
        }
        else
        {
            WriteTextOutput(allDiagnostics, settings.NoColor);
        }

        return allDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error) ? 1 : 0;
    }

    private static string[] ResolveFiles(string path, string[]? includePatterns, string[]? excludePatterns)
    {
        if (File.Exists(path))
            return [path];

        if (!Directory.Exists(path))
            return [];

        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

        if (excludePatterns is { Length: > 0 })
        {
            files = files.Where(f => !excludePatterns.Any(p => MatchGlob(f, p))).ToArray();
        }

        return files;
    }

    private static bool MatchGlob(string path, string pattern)
    {
        // Normalize separators
        path = path.Replace('\\', '/');
        pattern = pattern.Replace('\\', '/');

        return System.IO.Path.GetFileName(path).Contains(
            pattern.TrimStart('*', '/'),
            StringComparison.OrdinalIgnoreCase);
    }

    private static void WriteTextOutput(List<Diagnostic> diagnostics, bool noColor)
    {
        if (diagnostics.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]No issues found.[/]");
            return;
        }

        foreach (var d in diagnostics.OrderBy(d => d.Location.GetLineSpan().Path).ThenBy(d => d.Location.GetLineSpan().StartLinePosition.Line))
        {
            var span = d.Location.GetLineSpan();
            var severity = d.Severity switch
            {
                DiagnosticSeverity.Error => noColor ? "error" : "[red]error[/]",
                DiagnosticSeverity.Warning => noColor ? "warning" : "[yellow]warning[/]",
                _ => noColor ? "info" : "[blue]info[/]"
            };

            var location = $"{span.Path}({span.StartLinePosition.Line + 1},{span.StartLinePosition.Character + 1})";
            AnsiConsole.MarkupLine($"{location}: {severity} {d.Id}: {d.GetMessage()}");
        }

        AnsiConsole.MarkupLine($"\n[yellow]{diagnostics.Count} issue(s) found.[/]");
    }

    private static void WriteJsonOutput(List<Diagnostic> diagnostics)
    {
        var output = new
        {
            count = diagnostics.Count,
            issues = diagnostics.Select(d =>
            {
                var span = d.Location.GetLineSpan();
                return new
                {
                    id = d.Id,
                    severity = d.Severity.ToString().ToLowerInvariant(),
                    message = d.GetMessage(),
                    file = span.Path,
                    line = span.StartLinePosition.Line + 1,
                    column = span.StartLinePosition.Character + 1,
                };
            })
        };

        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(output, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
    }
}
