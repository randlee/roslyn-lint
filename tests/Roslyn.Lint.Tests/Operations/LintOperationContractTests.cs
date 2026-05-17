namespace Roslyn.Lint.Tests.Operations;

using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Lint.Contracts;
using Roslyn.Lint.Operations;
using Xunit;

public sealed class LintOperationContractTests
{
    [Fact]
    public async Task ExecuteAsync_InvalidPath_ReturnsValidationEnvelope()
    {
        var operation = new LintOperation(new FakeWorkspaceAdapter(pathExists: false));
        var request = new LintRequest("missing", Json: true, ImmutableArray<string>.Empty, ImmutableArray<string>.Empty, NoColor: false);

        var envelope = await operation.ExecuteAsync(request, CancellationToken.None);

        envelope.Success.Should().BeFalse();
        envelope.Operation.Should().Be("lint.run");
        envelope.Error.Should().NotBeNull();
        envelope.Error!.Kind.Should().Be(CliErrorKind.Validation);
        envelope.Error.Code.Should().Be("CLI.INVALID_PATH");
    }

    [Fact]
    public async Task ExecuteAsync_Diagnostics_ReturnsStableResultPayload()
    {
        var diagnostic = Diagnostic.Create(
            new DiagnosticDescriptor("DM002", "Forbidden string", "Forbidden string", "roslyn-lint.Organization", DiagnosticSeverity.Warning, isEnabledByDefault: true),
            Location.Create("Sample.cs", new TextSpan(0, 1), new LinePositionSpan(new LinePosition(2, 4), new LinePosition(2, 5))));

        var operation = new LintOperation(new FakeWorkspaceAdapter(
            pathExists: true,
            files: ImmutableArray.Create("/repo/Sample.cs"),
            diagnosticsByFile: new Dictionary<string, ImmutableArray<Diagnostic>>
            {
                ["/repo/Sample.cs"] = ImmutableArray.Create(diagnostic),
            }));

        var request = new LintRequest("/repo", Json: true, ImmutableArray<string>.Empty, ImmutableArray<string>.Empty, NoColor: false);

        var envelope = await operation.ExecuteAsync(request, CancellationToken.None);

        envelope.Success.Should().BeTrue();
        envelope.Result.Should().NotBeNull();
        envelope.Result!.Count.Should().Be(1);
        envelope.Result.Issues.Should().ContainSingle();
        envelope.Result.Issues[0].Id.Should().Be("DM002");
        envelope.Result.Issues[0].Severity.Should().Be("warning");
        envelope.Result.Issues[0].File.Should().Be("Sample.cs");
        envelope.Result.Issues[0].Line.Should().Be(3);
        envelope.Result.Issues[0].Column.Should().Be(5);
    }

    private sealed class FakeWorkspaceAdapter(
        bool pathExists,
        ImmutableArray<string> files = default,
        Dictionary<string, ImmutableArray<Diagnostic>>? diagnosticsByFile = null) : ILintWorkspaceAdapter
    {
        private readonly ImmutableArray<string> files = files.IsDefault ? ImmutableArray<string>.Empty : files;
        private readonly Dictionary<string, ImmutableArray<Diagnostic>> diagnosticsByFile = diagnosticsByFile ?? [];

        public bool PathExists(string path) => pathExists;

        public Task<ImmutableArray<string>> ResolveFilesAsync(LintRequest request, CancellationToken cancellationToken)
            => Task.FromResult(files);

        public Task<ImmutableArray<Diagnostic>> AnalyzeFileAsync(string filePath, CancellationToken cancellationToken)
            => Task.FromResult(diagnosticsByFile.GetValueOrDefault(filePath, ImmutableArray<Diagnostic>.Empty));
    }
}
