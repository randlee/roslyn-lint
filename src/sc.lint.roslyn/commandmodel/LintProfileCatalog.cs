namespace sc.lint.roslyn.commandmodel;

using sc.lint.roslyn.abstractions;

internal static class LintProfileCatalog
{
    private static readonly ProfileDefinition[] Definitions =
    [
        new(
            LintProfile.Fast,
            ScLintRoslynConstants.Commands.LintFast,
            "Low-latency local lint gate.",
            [ScLintRoslynConstants.Tools.DeMagicId]),
        new(
            LintProfile.Full,
            ScLintRoslynConstants.Commands.LintFull,
            "Stronger pre-push lint gate.",
            [ScLintRoslynConstants.Tools.DeMagicId]),
        new(
            LintProfile.Ci,
            ScLintRoslynConstants.Commands.LintCi,
            "Lint-only CI-parity gate.",
            [ScLintRoslynConstants.Tools.DeMagicId]),
    ];

    public static IReadOnlyList<ProfileDefinition> All => Definitions;

    public static ProfileDefinition GetRequired(LintProfile profile)
        => Definitions.Single(definition => definition.Profile == profile);
}

internal sealed record ProfileDefinition(
    LintProfile Profile,
    string CommandId,
    string Description,
    IReadOnlyList<ToolId> Members);
