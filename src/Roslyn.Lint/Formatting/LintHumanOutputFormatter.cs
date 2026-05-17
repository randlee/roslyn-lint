namespace Roslyn.Lint.Formatting;

using System.Collections.Immutable;
using Roslyn.Lint.Contracts;
using Spectre.Console;

public sealed class LintHumanOutputFormatter(bool noColor) : IHumanOutputFormatter<LintResult>
{
    public void WriteSuccess(LintResult response, ImmutableArray<CliWarning> warnings)
    {
        foreach (var warning in warnings)
            WriteWarning(warning);

        if (response.Count == 0)
        {
            WriteLine("[green]No issues found.[/]", "No issues found.");
            return;
        }

        foreach (var issue in response.Issues)
        {
            var severity = issue.Severity switch
            {
                "error" => noColor ? "error" : "[red]error[/]",
                "warning" => noColor ? "warning" : "[yellow]warning[/]",
                _ => noColor ? "info" : "[blue]info[/]",
            };

            var location = $"{issue.File}({issue.Line},{issue.Column})";
            WriteLine(
                $"{Markup.Escape(location)}: {severity} {Markup.Escape(issue.Id)}: {Markup.Escape(issue.Message)}",
                $"{location}: {issue.Severity} {issue.Id}: {issue.Message}");
        }

        WriteLine($"\n[yellow]{response.Count} issue(s) found.[/]", $"\n{response.Count} issue(s) found.");
    }

    public void WriteFailure(CliError error, ImmutableArray<CliWarning> warnings)
    {
        foreach (var warning in warnings)
            WriteWarning(warning);

        WriteLine(
            $"[red]error[/] {Markup.Escape(error.Code)}: {Markup.Escape(error.Message)}",
            $"error {error.Code}: {error.Message}");

        if (!string.IsNullOrWhiteSpace(error.SuggestedAction))
            WriteLine($"[yellow]suggested action:[/] {Markup.Escape(error.SuggestedAction)}", $"suggested action: {error.SuggestedAction}");
    }

    private void WriteWarning(CliWarning warning)
        => WriteLine(
            $"[yellow]warning[/] {Markup.Escape(warning.Code)}: {Markup.Escape(warning.Message)}",
            $"warning {warning.Code}: {warning.Message}");

    private void WriteLine(string markupText, string plainText)
    {
        if (noColor)
        {
            Console.WriteLine(plainText);
            return;
        }

        AnsiConsole.MarkupLine(markupText);
    }
}
