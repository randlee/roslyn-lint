---
id: B2
title: local cli dogfooding
status: complete
branch: sprint/B2-v2
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B2-v2
target: integration/phase-B
---

# Sprint B2 - Local CLI Dogfooding

## Goal

- Start using `sc-lint-roslyn` itself in normal repository workflows.
- Systematically locate command-surface, JSON-contract, and usability gaps.
- File follow-up issues for anything that does not work exactly as expected.

## Hard Dependencies

- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/phase-B/sprint-B1.md`
- `src/sc.lint.roslyn.abstractions/`
- `src/sc.lint.roslyn/`

## Exact Targets

- `docs/phase-B/cli-dogfood-findings.md`
- `docs/phase-B/cli-dogfood-remediation-policy.md`
- `docs/phase-B/cli-follow-up-issues.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `src/sc.lint.roslyn.abstractions/`
- `src/sc.lint.roslyn/`
- `tests/sc.lint.roslyn.tests/`

## Deliverables

- `docs/phase-B/cli-dogfood-findings.md` inventories every dogfooded command
  family and workflow independently
- `docs/phase-B/cli-dogfood-remediation-policy.md` classifies every B2 finding
  independently
- `docs/phase-B/cli-follow-up-issues.md` records every predictability gap,
  contract surprise, ownership ambiguity, or usability defect independently
- `docs/sc-lint-roslyn/cli-contract.md` is updated if B2 finds contract-shape
  ambiguity that must be resolved at the documentation boundary
- the sprint records which abstractions and contract types must be carried into
  Phase C boundary-package planning
- the sprint records the initial CLI dogfooding enforcement posture explicitly

## Important Interfaces, Records/Structs, And Enums

- existing contract and seam inventory to evaluate during dogfooding:
  `ToolId`, `ToolDescriptor`, `ILintToolModule`,
  `ILintToolCommandHandler<TRequest, TResponse>`, `CliEnvelope<TResult>`,
  `CliError`, `CliDiagnostic`, `CliErrorKind`, `LintToolRequest`,
  `LintToolResult`, `LintFinding`, `ViewRequest`, `ViewResult`,
  `CheckRequest`, `CheckResult`, `ClippyRequest`, `ClippyResult`,
  `CiRequest`, `CiResult`, `VersionResult`

Important command and contract signatures:

```bash
sc-lint-roslyn version --json
sc-lint-roslyn lint demagic --json
sc-lint-roslyn view tools --json
sc-lint-roslyn check --json
sc-lint-roslyn clippy --json
sc-lint-roslyn ci --json
```

```json
{
  "ok": true,
  "command": "lint.demagic",
  "data": {},
  "diagnostics": []
}
```

```json
{
  "ok": false,
  "command": "check",
  "error": {
    "kind": "capability",
    "code": "CLI.CAPABILITY_ERROR",
    "message": "Required tool is unavailable"
  },
  "diagnostics": []
}
```

## Required Work

- dogfood `sc-lint-roslyn` through repository-owned workflows and command paths
  rather than only through narrow unit tests
- record every command-surface, JSON-contract, and usability finding in
  `docs/phase-B/cli-dogfood-findings.md`
- classify each finding in `docs/phase-B/cli-dogfood-remediation-policy.md` as:
  immediate fix, contract clarification, boundary-guardrail change, or later-
  phase remediation
- create `docs/phase-B/cli-follow-up-issues.md` for every predictability gap,
  contract surprise, or ownership ambiguity discovered during B2
- update `docs/sc-lint-roslyn/cli-contract.md` when B2 discovers ambiguity in
  the documented machine contract rather than only in implementation behavior
- identify which abstractions and contract items should be escalated into the
  later Phase C boundary-package planning sprint, but do not treat that
  planning work as part of B2 closeout
- every listed deliverable in this sprint is expected to land at a
  production-ready level for B2's claimed scope; no deliverable may close in a
  shape-only, placeholder, or “document later” state

## Non-Closure Items

- B2 does not implement new CLI families or new backend packages
- B2 does not begin Phase C boundary-package implementation
- B2 does not broaden the stable command contract beyond the existing shipped
  command families

## Acceptance Criteria

- `docs/phase-B/cli-dogfood-findings.md` exists and covers the dogfooded CLI
  workflows
- `docs/phase-B/cli-dogfood-remediation-policy.md` exists and classifies every
  B2 finding disposition
- `docs/phase-B/cli-follow-up-issues.md` exists and contains every known CLI
  predictability or contract gap found during B2
- `docs/sc-lint-roslyn/cli-contract.md` is updated when B2 resolves a
  documentation-level contract ambiguity
- the sprint records which abstractions and contract types should later be
  brought under Phase C boundary-package planning
- the sprint records the dogfooded command families and workflows explicitly,
  not just “the CLI” as an umbrella reference

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
