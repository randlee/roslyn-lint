namespace Roslyn.Lint.Formatting;

using System.Collections.Immutable;
using Roslyn.Lint.Contracts;

public interface IHumanOutputFormatter<TResponse>
{
    void WriteSuccess(TResponse response, ImmutableArray<CliWarning> warnings);

    void WriteFailure(CliError error, ImmutableArray<CliWarning> warnings);
}
