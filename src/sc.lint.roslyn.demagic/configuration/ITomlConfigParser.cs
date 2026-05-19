namespace sc.lint.roslyn.demagic.configuration;

using System.Collections.Immutable;

public interface ITomlConfigParser
{
    bool TryParse(string content, out DeMagicConfig config, out ImmutableArray<string> errors);
}
