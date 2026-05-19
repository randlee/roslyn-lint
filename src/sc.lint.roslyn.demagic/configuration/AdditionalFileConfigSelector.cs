namespace sc.lint.roslyn.demagic.configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public sealed class AdditionalFileConfigSelector : IAdditionalFileConfigSelector
{
    private static readonly string[] SupportedFileNames =
    [
        "config-test.toml",
        "config-src.toml",
    ];

    public AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles)
    {
        foreach (var fileName in SupportedFileNames)
        {
            var match = additionalFiles.FirstOrDefault(file =>
                file.Path.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            if (match is null)
                continue;

            var content = match.GetText()?.ToString();
            if (content is null)
                continue;

            return new AdditionalFileConfigSelection(match.Path, content);
        }

        return null;
    }
}
