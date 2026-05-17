namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public sealed class DeMagicConfigLoader
{
    private readonly IAdditionalFileConfigSelector selector;
    private readonly ITomlConfigParser parser;

    public DeMagicConfigLoader()
        : this(new AdditionalFileConfigSelector(), new SimpleTomlConfigParser())
    {
    }

    public DeMagicConfigLoader(IAdditionalFileConfigSelector selector, ITomlConfigParser parser)
    {
        this.selector = selector;
        this.parser = parser;
    }

    public bool TryLoad(
        ImmutableArray<AdditionalText> additionalFiles,
        out DeMagicConfig config,
        out ImmutableArray<string> errors)
    {
        var selection = selector.Select(additionalFiles);
        if (selection is null)
        {
            config = DeMagicConfig.Disabled;
            errors = ImmutableArray<string>.Empty;
            return false;
        }

        return parser.TryParse(selection.Value.Content, out config, out errors);
    }
}
