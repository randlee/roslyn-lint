namespace Roslyn.Lint.Operations;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Roslyn.Lint.Contracts;

public interface ILintWorkspaceAdapter
{
    bool PathExists(string path);

    Task<ImmutableArray<string>> ResolveFilesAsync(LintRequest request, CancellationToken cancellationToken);

    Task<ImmutableArray<Diagnostic>> AnalyzeFileAsync(string filePath, CancellationToken cancellationToken);
}
