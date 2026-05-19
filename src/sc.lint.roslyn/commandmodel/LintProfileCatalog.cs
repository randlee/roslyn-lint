namespace sc.lint.roslyn.commandmodel;

using sc.lint.roslyn.abstractions;

internal static class LintProfileCatalog
{
    private static readonly ProfileDefinition[] Definitions =
    [
        new(
            LintProfile.Fast,
            "lint.fast",
            "Low-latency local lint gate.",
            [new ToolId("demagic")]),
        new(
            LintProfile.Full,
            "lint.full",
            "Stronger pre-push lint gate.",
            [new ToolId("demagic")]),
        new(
            LintProfile.Ci,
            "lint.ci",
            "Lint-only CI-parity gate.",
            [new ToolId("demagic")]),
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
