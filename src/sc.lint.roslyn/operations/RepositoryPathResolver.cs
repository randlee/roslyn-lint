namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions;

internal static class RepositoryPathResolver
{
    public static string ResolveRepoRoot(string targetPath)
    {
        var current = Directory.Exists(targetPath)
            ? new DirectoryInfo(targetPath)
            : new FileInfo(targetPath).Directory;

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, ScLintRoslynConstants.Suite.SolutionFileName)))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException($"Could not locate {ScLintRoslynConstants.Suite.SolutionFileName} from '{targetPath}'.");
    }
}
