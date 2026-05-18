namespace Roslyn.DeMagic.Tests.Testing;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

internal sealed class TestAdditionalText(string path, string content) : AdditionalText
{
    public override string Path { get; } = path;

    public override SourceText? GetText(CancellationToken cancellationToken = default)
        => SourceText.From(content);
}
