namespace Roslyn.Lint.Operations;

internal static class RepositoryPathResolver
{
    public static string ResolveRepoRoot(string targetPath)
    {
        var current = Directory.Exists(targetPath)
            ? new DirectoryInfo(targetPath)
            : new FileInfo(targetPath).Directory;

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "roslyn-lint.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException($"Could not locate roslyn-lint.sln from '{targetPath}'.");
    }
}
