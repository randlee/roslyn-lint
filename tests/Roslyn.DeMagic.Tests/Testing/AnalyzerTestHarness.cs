namespace Roslyn.DeMagic.Tests.Testing;

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

internal static class AnalyzerTestHarness
{
    private static readonly ImmutableArray<MetadataReference> MetadataReferences = CreateMetadataReferences();

    public static async Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(
        DiagnosticAnalyzer analyzer,
        string code,
        string sourcePath,
        params TestAdditionalText[] additionalFiles)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code, path: sourcePath);
        var compilation = CSharpCompilation.Create(
            assemblyName: "Roslyn.DeMagic.Tests",
            syntaxTrees: [syntaxTree],
            references: MetadataReferences,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var analyzerOptions = new AnalyzerOptions(additionalFiles.Cast<AdditionalText>().ToImmutableArray());
        var withAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer), analyzerOptions);

        return await withAnalyzers.GetAnalyzerDiagnosticsAsync();
    }

    public static string LoadFixture(string relativePath)
    {
        var normalizedPath = relativePath.Replace('/', Path.DirectorySeparatorChar);
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "TestData", normalizedPath);
        return File.ReadAllText(fixturePath);
    }

    private static ImmutableArray<MetadataReference> CreateMetadataReferences()
    {
        var trustedPlatformAssemblies = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
        if (string.IsNullOrWhiteSpace(trustedPlatformAssemblies))
            throw new InvalidOperationException("TRUSTED_PLATFORM_ASSEMBLIES was not available for analyzer tests.");

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

        return trustedPlatformAssemblies
            .Split(Path.PathSeparator)
            .Where(path => requiredAssemblies.Contains(Path.GetFileName(path)))
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToImmutableArray();
    }
}
