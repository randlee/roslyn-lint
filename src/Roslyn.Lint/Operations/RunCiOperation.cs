namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Backends;
using Roslyn.Lint.CommandModel;
using Roslyn.Lint.Contracts;

public sealed class RunCiOperation : ICiOperation
{
    private readonly ILintToolOperation lintToolOperation;
    private readonly IDotnetCommandRunner dotnetCommandRunner;

    public RunCiOperation(ILintToolOperation lintToolOperation, IDotnetCommandRunner dotnetCommandRunner)
    {
        this.lintToolOperation = lintToolOperation;
        this.dotnetCommandRunner = dotnetCommandRunner;
    }

    public async Task<CiResult> ExecuteAsync(CiRequest request, CancellationToken cancellationToken)
    {
        var repoRoot = RepositoryPathResolver.ResolveRepoRoot(request.TargetPath);
        var lintResult = await LintProfileRunner.ExecuteAsync(
            LintProfile.Ci,
            repoRoot,
            lintToolOperation,
            cancellationToken);

        if (lintResult.FindingCount > 0)
        {
            throw new CiLintGateFailedException(lintResult);
        }

        var testResult = await dotnetCommandRunner.RunAsync(
            repoRoot,
            [
                "test",
                "tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj",
                "--configuration",
                request.Configuration,
                "--verbosity",
                "normal",
            ],
            cancellationToken);

        if (!testResult.Succeeded)
        {
            throw new DotnetCommandFailedException("test", testResult);
        }

        return new CiResult(
            repoRoot,
            request.Configuration,
            "pass",
            lintResult,
            [new WorkflowStepResult("test", "dotnet", testResult.ArgumentsDisplay, testResult.ExitCode, "pass")]);
    }
}
