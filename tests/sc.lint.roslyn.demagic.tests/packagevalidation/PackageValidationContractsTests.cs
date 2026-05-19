namespace sc.lint.roslyn.demagic.tests.packagevalidation;

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
            "sc-lint-roslyn-demagic-package-expected-diagnostics.json");

        var json = File.ReadAllText(Path.GetFullPath(manifestPath));
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        var manifest = JsonSerializer.Deserialize<PackageValidationManifest>(json, options);

        manifest.Should().NotBeNull();
        manifest!.PackageId.Should().Be("sc-lint-roslyn-demagic");
        manifest.PackageVersion.Should().Be("0.1.1");
        manifest.ExpectedDiagnostics.Should().HaveCount(9);
        manifest.ExpectedDiagnostics.Select(diagnostic => diagnostic.SampleKind)
            .Should().OnlyContain(kind => kind == PackageValidationSampleKind.Positive);
        manifest.ExpectedDiagnostics.Should().ContainSingle(diagnostic =>
            diagnostic.File == "samples/dm001/UnsuppressedConstControl.cs" &&
            diagnostic.Id == "DM001" &&
            diagnostic.Severity == "warning");
        manifest.ExpectedDiagnostics.Should().ContainSingle(diagnostic =>
            diagnostic.File == "samples/dm002/UnsuppressedLiteralControl.cs" &&
            diagnostic.Id == "DM002" &&
            diagnostic.Severity == "warning");
        manifest.ExpectedCleanFiles.Should().HaveCount(4);
    }

    [Fact]
    public void ResultRecord_PreservesValidationCollections()
    {
        var result = new PackageValidationResult(
            Success: false,
            MissingExpectedDiagnostics: ["samples/dm001/PublicConstOutsideDesignatedFile.cs|DM001|warning"],
            UnexpectedDiagnostics: ["samples/dm002/ExactMatchConstField.cs|DM002|warning"],
            DiagnosticsReportedForCleanFiles: ["samples/dm002/CompliantLiteral.cs|DM002|warning"]);

        result.Success.Should().BeFalse();
        result.MissingExpectedDiagnostics.Should().ContainSingle();
        result.UnexpectedDiagnostics.Should().ContainSingle();
        result.DiagnosticsReportedForCleanFiles.Should().ContainSingle();
    }
}
