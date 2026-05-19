namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.backends;
using sc.lint.roslyn.contracts;

public sealed class RunCheckOperation : ICheckOperation
{
    private readonly IDotnetCommandRunner dotnetCommandRunner;

    public RunCheckOperation(IDotnetCommandRunner dotnetCommandRunner)
    {
        this.dotnetCommandRunner = dotnetCommandRunner;
    }

    public async Task<CheckResult> ExecuteAsync(CheckRequest request, CancellationToken cancellationToken)
    {
        var repoRoot = RepositoryPathResolver.ResolveRepoRoot(request.TargetPath);
        var buildResult = await dotnetCommandRunner.RunAsync(
            repoRoot,
            ["build", "sc-lint-roslyn.sln", "--configuration", request.Configuration, "--no-restore"],
            cancellationToken);

        if (!buildResult.Succeeded)
        {
            throw new DotnetCommandFailedException("build", buildResult);
        }

        return new CheckResult(
            repoRoot,
            request.Configuration,
            "pass",
            [CreateStep("build", buildResult)]);
    }

    private static WorkflowStepResult CreateStep(string name, DotnetCommandResult result)
        => new(name, "dotnet", result.ArgumentsDisplay, result.ExitCode, "pass");
}
