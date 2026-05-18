namespace Roslyn.DeMagic.Tests.PackageValidation;

using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Xunit;

public sealed class PackageValidationContractsTests
{
    [Fact]
    public void ManifestJson_DeserializesIntoPackageValidationManifest()
    {
        var manifestPath = Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "..",
            "eng",
            "roslyn-demagic-package-expected-diagnostics.json");

        var json = File.ReadAllText(Path.GetFullPath(manifestPath));
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        var manifest = JsonSerializer.Deserialize<PackageValidationManifest>(json, options);

        manifest.Should().NotBeNull();
        manifest!.PackageId.Should().Be("Roslyn.DeMagic");
        manifest.PackageVersion.Should().Be("0.1.0");
        manifest.ExpectedDiagnostics.Should().HaveCount(8);
        manifest.ExpectedDiagnostics.Select(diagnostic => diagnostic.SampleKind)
            .Should().OnlyContain(kind => kind == PackageValidationSampleKind.Positive);
        manifest.ExpectedDiagnostics.Should().ContainSingle(diagnostic =>
            diagnostic.File == "Samples/DM001/UnsuppressedConstControl.cs" &&
            diagnostic.Id == "DM001" &&
            diagnostic.Severity == "warning");
        manifest.ExpectedCleanFiles.Should().HaveCount(4);
    }

    [Fact]
    public void ResultRecord_PreservesValidationCollections()
    {
        var result = new PackageValidationResult(
            Success: false,
            MissingExpectedDiagnostics: ["Samples/DM001/PublicConstOutsideDesignatedFile.cs|DM001|warning"],
            UnexpectedDiagnostics: ["Samples/DM002/ExactMatchConstField.cs|DM002|warning"],
            DiagnosticsReportedForCleanFiles: ["Samples/DM002/CompliantLiteral.cs|DM002|warning"]);

        result.Success.Should().BeFalse();
        result.MissingExpectedDiagnostics.Should().ContainSingle();
        result.UnexpectedDiagnostics.Should().ContainSingle();
        result.DiagnosticsReportedForCleanFiles.Should().ContainSingle();
    }
}
