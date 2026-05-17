namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public interface IDeMagicConfigLoader
{
    bool TryLoad(
        ImmutableArray<AdditionalText> additionalFiles,
        out DeMagicConfig config,
        out ImmutableArray<string> errors);
}
