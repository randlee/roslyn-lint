namespace Roslyn.Lint.Operations;

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.DeMagic.Analyzers;
using Roslyn.Lint.Contracts;

public sealed class LintWorkspaceAdapter : ILintWorkspaceAdapter
{
    private static readonly ImmutableArray<DiagnosticAnalyzer> Analyzers =
    [
        new DM002ForbiddenStringLiteralAnalyzer(),
    ];

    public bool PathExists(string path) => File.Exists(path) || Directory.Exists(path);

    public Task<ImmutableArray<string>> ResolveFilesAsync(LintRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string[] files;
        if (File.Exists(request.Path))
        {
            files = [request.Path];
        }
        else if (!Directory.Exists(request.Path))
        {
            files = [];
        }
        else
        {
            files = Directory.GetFiles(request.Path, "*.cs", SearchOption.AllDirectories);
        }

        if (!request.IncludePatterns.IsDefaultOrEmpty)
        {
            files = files.Where(file => request.IncludePatterns.Any(pattern => MatchGlob(file, pattern))).ToArray();
        }

        if (!request.ExcludePatterns.IsDefaultOrEmpty)
        {
            files = files.Where(file => !request.ExcludePatterns.Any(pattern => MatchGlob(file, pattern))).ToArray();
        }

        return Task.FromResult(files.ToImmutableArray());
    }

    public async Task<ImmutableArray<Diagnostic>> AnalyzeFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var source = await File.ReadAllTextAsync(filePath, cancellationToken);
        var tree = CSharpSyntaxTree.ParseText(source, path: filePath, cancellationToken: cancellationToken);

        var compilation = CSharpCompilation.Create(
            assemblyName: System.IO.Path.GetFileNameWithoutExtension(filePath),
            syntaxTrees: [tree],
            references: [],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var compilationWithAnalyzers = compilation.WithAnalyzers(Analyzers);
        var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken);
        return diagnostics.ToImmutableArray();
    }

    private static bool MatchGlob(string path, string pattern)
    {
        var normalizedPath = path.Replace('\\', '/');
        var normalizedPattern = pattern.Replace('\\', '/');
        var target = normalizedPattern.Contains('/', StringComparison.Ordinal)
            ? normalizedPath
            : System.IO.Path.GetFileName(normalizedPath);

        var regexPattern = "^" + Regex.Escape(normalizedPattern)
            .Replace(@"\*\*", "__DOUBLE_STAR__")
            .Replace(@"\*", "[^/]*")
            .Replace(@"\?", "[^/]")
            .Replace("__DOUBLE_STAR__", ".*") + "$";

        return Regex.IsMatch(target, regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
