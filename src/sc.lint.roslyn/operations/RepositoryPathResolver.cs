namespace sc.lint.roslyn.operations;

internal static class RepositoryPathResolver
{
    public static string ResolveRepoRoot(string targetPath)
    {
        var current = Directory.Exists(targetPath)
            ? new DirectoryInfo(targetPath)
            : new FileInfo(targetPath).Directory;

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "sc-lint-roslyn.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException($"Could not locate sc-lint-roslyn.sln from '{targetPath}'.");
    }
}
