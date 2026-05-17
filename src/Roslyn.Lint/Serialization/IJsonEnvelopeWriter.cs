namespace Roslyn.Lint.Serialization;

using Roslyn.Lint.Contracts;

public interface IJsonEnvelopeWriter
{
    void Write<TResult>(CliEnvelope<TResult> envelope);
}
