namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public interface IAdditionalFileConfigSelector
{
    AdditionalFileConfigSelection? Select(ImmutableArray<AdditionalText> additionalFiles);
}
