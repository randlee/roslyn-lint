namespace Roslyn.Lint.Contracts;

using System.Collections.Immutable;

public sealed record LintResult(int Count, ImmutableArray<LintIssue> Issues);
