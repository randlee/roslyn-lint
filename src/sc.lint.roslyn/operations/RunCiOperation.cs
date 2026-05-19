namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.backends;
using sc.lint.roslyn.commandmodel;
using sc.lint.roslyn.contracts;

public sealed class RunCiOperation : ICiOperation
{
    private static readonly (string Name, string ProjectPath)[] TestProjects =
    [
        ("test-demagic", "tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj"),
        ("test-cli", "tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj"),
    ];

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

        var steps = new List<WorkflowStepResult>(TestProjects.Length);

        foreach (var (name, projectPath) in TestProjects)
        {
            var testResult = await dotnetCommandRunner.RunAsync(
                repoRoot,
                [
                    "test",
                    projectPath,
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

            steps.Add(new WorkflowStepResult(name, "dotnet", testResult.ArgumentsDisplay, testResult.ExitCode, "pass"));
        }

        return new CiResult(
            repoRoot,
            request.Configuration,
            "pass",
            lintResult,
            steps);
    }
}
