namespace sc.lint.roslyn.operations;

using sc.lint.roslyn.abstractions;
using sc.lint.roslyn.backends;
using sc.lint.roslyn.contracts;

public sealed class RunClippyOperation : IClippyOperation
{
    private readonly IDotnetCommandRunner dotnetCommandRunner;

    public RunClippyOperation(IDotnetCommandRunner dotnetCommandRunner)
    {
        this.dotnetCommandRunner = dotnetCommandRunner;
    }

    public async Task<ClippyResult> ExecuteAsync(ClippyRequest request, CancellationToken cancellationToken)
    {
        var repoRoot = RepositoryPathResolver.ResolveRepoRoot(request.TargetPath);
        var buildResult = await dotnetCommandRunner.RunAsync(
            repoRoot,
            ["build", ScLintRoslynConstants.Suite.SolutionFileName, "--configuration", request.Configuration, "--no-restore", "-warnaserror"],
            cancellationToken);

        if (!buildResult.Succeeded)
        {
            throw new DotnetCommandFailedException("build", buildResult);
        }

        var formatResult = await dotnetCommandRunner.RunAsync(
            repoRoot,
            ["format", ScLintRoslynConstants.Suite.SolutionFileName, "--verify-no-changes", "--no-restore"],
            cancellationToken);

        if (!formatResult.Succeeded)
        {
            throw new DotnetCommandFailedException("format", formatResult);
        }

        return new ClippyResult(
            repoRoot,
            request.Configuration,
            "pass",
            [CreateStep("build", buildResult), CreateStep("format", formatResult)]);
    }

    private static WorkflowStepResult CreateStep(string name, DotnetCommandResult result)
        => new(name, "dotnet", result.ArgumentsDisplay, result.ExitCode, "pass");
}
