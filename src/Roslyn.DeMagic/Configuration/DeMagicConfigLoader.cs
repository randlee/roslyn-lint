namespace Roslyn.DeMagic.Configuration;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

public sealed class DeMagicConfigLoader : IDeMagicConfigLoader
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
        try
        {
            var selection = selector.Select(additionalFiles);
            if (selection is null)
            {
                config = DeMagicConfig.Disabled;
                errors = ImmutableArray<string>.Empty;
                return false;
            }

            var success = parser.TryParse(selection.Value.Content, out config, out errors);
            if (success)
                return true;

            config = DeMagicConfig.Disabled;
            return false;
        }
        catch (Exception exception)
        {
            config = DeMagicConfig.Disabled;
            errors = ImmutableArray.Create(exception.Message);
            return false;
        }
    }
}
