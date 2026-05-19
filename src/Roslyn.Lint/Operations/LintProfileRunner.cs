namespace Roslyn.Lint.Operations;

using Roslyn.Lint.Abstractions.Contracts;
using Roslyn.Lint.CommandModel;
using Roslyn.Lint.Contracts;

internal static class LintProfileRunner
{
    public static async Task<LintProfileResult> ExecuteAsync(
        LintProfile profile,
        string targetPath,
        ILintToolOperation lintToolOperation,
        CancellationToken cancellationToken)
    {
        var definition = LintProfileCatalog.GetRequired(profile);
        var findings = new List<LintFinding>();

        foreach (var member in definition.Members)
        {
            var result = await lintToolOperation.ExecuteAsync(
                new LintToolRequest(member, targetPath),
                cancellationToken);
            findings.AddRange(result.Findings);
        }

        return new LintProfileResult(
            profile.ToString().ToLowerInvariant(),
            targetPath,
            findings.Count == 0 ? "pass" : "findings",
            findings.Count,
            definition.Members.Select(member => member.Value).ToArray(),
            findings);
    }
}
