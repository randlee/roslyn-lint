namespace sc.lint.roslyn.demagic.configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public interface IAdditionalFileConfigSelector
{
    AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles);
}
