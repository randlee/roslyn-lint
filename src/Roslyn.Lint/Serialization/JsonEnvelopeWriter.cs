namespace Roslyn.Lint.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Roslyn.Lint.Contracts;

public sealed class JsonEnvelopeWriter : IJsonEnvelopeWriter
{
    public static JsonSerializerOptions DefaultOptions { get; } = CreateDefaultOptions();

    public void Write<TResult>(CliEnvelope<TResult> envelope)
        => Console.WriteLine(JsonSerializer.Serialize(envelope, DefaultOptions));

    private static JsonSerializerOptions CreateDefaultOptions()
        => new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            },
            WriteIndented = true,
        };
}
