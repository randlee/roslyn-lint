namespace Roslyn.Lint.Operations;

using System.Collections.Immutable;
using Roslyn.Lint.Contracts;

public sealed class LintOperation(ILintWorkspaceAdapter workspaceAdapter) : ICommandOperation<LintRequest, LintResult>
{
    private const string OperationName = "lint.run";

    public async Task<CliEnvelope<LintResult>> ExecuteAsync(LintRequest request, CancellationToken cancellationToken)
    {
        if (!workspaceAdapter.PathExists(request.Path))
        {
            return CliEnvelope<LintResult>.Fail(
                OperationName,
                new CliError(
                    CliErrorKind.Validation,
                    "CLI.INVALID_PATH",
                    "Path does not exist",
                    new Dictionary<string, string> { ["path"] = request.Path },
                    "Provide an existing file or directory path"));
        }

        try
        {
            var files = await workspaceAdapter.ResolveFilesAsync(request, cancellationToken);
            if (files.IsDefaultOrEmpty)
                return CliEnvelope<LintResult>.Ok(OperationName, new LintResult(0, ImmutableArray<LintIssue>.Empty));

            var issues = ImmutableArray.CreateBuilder<LintIssue>();
            foreach (var file in files)
            {
                var diagnostics = await workspaceAdapter.AnalyzeFileAsync(file, cancellationToken);
                foreach (var diagnostic in diagnostics)
                {
                    var span = diagnostic.Location.GetLineSpan();
                    issues.Add(new LintIssue(
                        diagnostic.Id,
                        diagnostic.Severity.ToString().ToLowerInvariant(),
                        diagnostic.GetMessage(),
                        span.Path,
                        span.StartLinePosition.Line + 1,
                        span.StartLinePosition.Character + 1));
                }
            }

            var orderedIssues = issues
                .ToImmutable()
                .OrderBy(issue => issue.File, StringComparer.Ordinal)
                .ThenBy(issue => issue.Line)
                .ThenBy(issue => issue.Column)
                .ToImmutableArray();

            return CliEnvelope<LintResult>.Ok(OperationName, new LintResult(orderedIssues.Length, orderedIssues));
        }
        catch (Exception exception)
        {
            return CliEnvelope<LintResult>.Fail(
                OperationName,
                new CliError(
                    CliErrorKind.Internal,
                    "CLI.LINT_FAILED",
                    "Lint execution failed",
                    new Dictionary<string, string> { ["exceptionType"] = exception.GetType().Name },
                    exception.Message));
        }
    }
}
