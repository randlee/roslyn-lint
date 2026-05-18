namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Roslyn.DeMagic.Patterns;

public sealed record DeMagicConfig(Dm001Options Dm001, Dm002Options Dm002)
{
    public static DeMagicConfig Disabled { get; } = new(
        new Dm001Options(false, ConfiguredSeverity.Warning, null, null),
        new Dm002Options(false, ConfiguredSeverity.Warning, ImmutableArray<ForbiddenPattern>.Empty, false));
}
