namespace sc.lint.roslyn.abstractions.contracts;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(CliErrorKindJsonConverter))]
public enum CliErrorKind
{
    Usage,
    Config,
    Capability,
    BackendFailure,
    BackendProtocol,
    Internal,
}

public sealed class CliErrorKindJsonConverter : JsonConverter<CliErrorKind>
{
    public override CliErrorKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() switch
        {
            "usage" => CliErrorKind.Usage,
            "config" => CliErrorKind.Config,
            "capability" => CliErrorKind.Capability,
            "backend_failure" => CliErrorKind.BackendFailure,
            "backend_protocol" => CliErrorKind.BackendProtocol,
            "internal" => CliErrorKind.Internal,
            var value => throw new JsonException($"Unsupported CLI error kind '{value}'."),
        };

    public override void Write(Utf8JsonWriter writer, CliErrorKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            CliErrorKind.Usage => "usage",
            CliErrorKind.Config => "config",
            CliErrorKind.Capability => "capability",
            CliErrorKind.BackendFailure => "backend_failure",
            CliErrorKind.BackendProtocol => "backend_protocol",
            CliErrorKind.Internal => "internal",
            _ => throw new JsonException($"Unsupported CLI error kind '{value}'."),
        });
    }
}
