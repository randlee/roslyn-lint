namespace sc.lint.roslyn.demagic.lint;

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using sc.lint.roslyn.demagic.analyzers;
using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.abstractions.contracts;

public sealed class DeMagicWorkspaceAdapter : ILintWorkspaceAdapter
{
    private static readonly ImmutableArray<MetadataReference> MetadataReferences = CreateMetadataReferences();

    public async Task<LintToolResult> ExecuteLintAsync(LintToolRequest request, CancellationToken cancellationToken)
    {
        var targetPath = Path.GetFullPath(request.TargetPath);
        var sourceFiles = ResolveSourceFiles(targetPath);
        var syntaxTrees = await LoadSyntaxTreesAsync(sourceFiles, cancellationToken);
        var additionalFiles = await LoadAdditionalFilesAsync(targetPath, cancellationToken);

        var compilation = CSharpCompilation.Create(
            assemblyName: "sc-lint-roslyn-demagic",
            syntaxTrees: syntaxTrees,
            references: MetadataReferences,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var analyzers = ImmutableArray.Create<DiagnosticAnalyzer>(
            new DM001ConstantConsolidationAnalyzer(),
            new DM002ForbiddenStringLiteralAnalyzer());
        var analyzerOptions = new AnalyzerOptions(additionalFiles.Cast<AdditionalText>().ToImmutableArray());
        var diagnostics = await compilation.WithAnalyzers(analyzers, analyzerOptions)
            .GetAnalyzerDiagnosticsAsync(cancellationToken);

        var findings = diagnostics
            .OrderBy(diagnostic => diagnostic.Location.SourceTree?.FilePath, StringComparer.OrdinalIgnoreCase)
            .ThenBy(diagnostic => diagnostic.Location.GetLineSpan().StartLinePosition.Line)
            .ThenBy(diagnostic => diagnostic.Location.GetLineSpan().StartLinePosition.Character)
            .ThenBy(diagnostic => diagnostic.Id, StringComparer.OrdinalIgnoreCase)
            .Select(MapFinding)
            .ToArray();

        return new LintToolResult(
            request.Tool.Value,
            targetPath,
            findings.Length == 0 ? "pass" : "findings",
            findings.Length,
            findings);
    }

    private static async Task<IReadOnlyList<SyntaxTree>> LoadSyntaxTreesAsync(
        IReadOnlyList<string> sourceFiles,
        CancellationToken cancellationToken)
    {
        var trees = new List<SyntaxTree>(sourceFiles.Count);
        foreach (var sourceFile in sourceFiles)
        {
            var sourceText = await File.ReadAllTextAsync(sourceFile, cancellationToken);
            trees.Add(CSharpSyntaxTree.ParseText(SourceText.From(sourceText), path: sourceFile));
        }

        return trees;
    }

    private static IReadOnlyList<string> ResolveSourceFiles(string targetPath)
    {
        if (File.Exists(targetPath))
        {
            return Path.GetExtension(targetPath).Equals(".cs", StringComparison.OrdinalIgnoreCase)
                ? [targetPath]
                : [];
        }

        if (!Directory.Exists(targetPath))
        {
            return [];
        }

        return Directory.EnumerateFiles(targetPath, "*.cs", SearchOption.AllDirectories)
            .Where(path => !IsInIgnoredDirectory(path))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static bool IsInIgnoredDirectory(string path)
    {
        return path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(segment => segment.Equals("bin", StringComparison.OrdinalIgnoreCase)
                || segment.Equals("obj", StringComparison.OrdinalIgnoreCase));
    }

    private static async Task<IReadOnlyList<FileAdditionalText>> LoadAdditionalFilesAsync(
        string targetPath,
        CancellationToken cancellationToken)
    {
        var root = ResolveConfigRoot(targetPath);
        if (root is null)
        {
            return [];
        }

        var configDirectory = Path.Combine(root, ".sc-lint-roslyn");
        if (!Directory.Exists(configDirectory))
        {
            return [];
        }

        var files = Directory.EnumerateFiles(configDirectory, "config-*.toml", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var results = new List<FileAdditionalText>(files.Length);
        foreach (var path in files)
        {
            var content = await File.ReadAllTextAsync(path, cancellationToken);
            results.Add(new FileAdditionalText(path, content));
        }

        return results;
    }

    private static string? ResolveConfigRoot(string targetPath)
    {
        var current = File.Exists(targetPath)
            ? new FileInfo(targetPath).Directory
            : new DirectoryInfo(targetPath);

        while (current is not null)
        {
            if (Directory.Exists(Path.Combine(current.FullName, ".sc-lint-roslyn")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return Directory.Exists(targetPath) ? targetPath : Path.GetDirectoryName(targetPath);
    }

    private static LintFinding MapFinding(Diagnostic diagnostic)
    {
        var span = diagnostic.Location.GetLineSpan();
        return new LintFinding(
            diagnostic.Id,
            ToSeverityString(diagnostic.Severity),
            diagnostic.Descriptor.Category,
            diagnostic.GetMessage(),
            span.Path ?? string.Empty,
            span.StartLinePosition.Line + 1,
            span.StartLinePosition.Character + 1);
    }

    private static string ToSeverityString(DiagnosticSeverity severity)
        => severity switch
        {
            DiagnosticSeverity.Hidden => "hidden",
            DiagnosticSeverity.Info => "info",
            DiagnosticSeverity.Warning => "warning",
            DiagnosticSeverity.Error => "error",
            _ => "warning",
        };

    private static ImmutableArray<MetadataReference> CreateMetadataReferences()
    {
        var trustedPlatformAssemblies = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
        if (string.IsNullOrWhiteSpace(trustedPlatformAssemblies))
        {
            throw new InvalidOperationException("TRUSTED_PLATFORM_ASSEMBLIES was not available.");
        }

        var requiredAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "mscorlib.dll",
            "netstandard.dll",
            "System.Private.CoreLib.dll",
            "System.Runtime.dll",
            "System.Runtime.Extensions.dll",
            "System.Console.dll",
            "System.Linq.dll",
            "System.Collections.dll",
            "System.Collections.Immutable.dll",
        };

        var references = trustedPlatformAssemblies
            .Split(Path.PathSeparator)
            .Where(path => requiredAssemblies.Contains(Path.GetFileName(path)))
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToList();

        references.Add(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));

        var distinct = new List<MetadataReference>();
        var seen = new HashSet<string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var reference in references)
        {
            if (seen.Add(reference.Display))
            {
                distinct.Add(reference);
            }
        }

        return distinct.ToImmutableArray();
    }

    private sealed class FileAdditionalText(string path, string content) : AdditionalText
    {
        public override string Path { get; } = path;

        public override SourceText GetText(CancellationToken cancellationToken = default)
            => SourceText.From(content);
    }
}
