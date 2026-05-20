---
id: B2
title: local cli dogfooding
status: planned
branch: sprint/B2
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B2
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
- `src/sc.lint.roslyn.abstractions/`
- `src/sc.lint.roslyn/`
- `tests/sc.lint.roslyn.tests/`

## Important Interfaces, Records/Structs, And Enums

- existing contract and seam inventory to evaluate during dogfooding:
  `ToolId`, `ToolDescriptor`, `ILintToolModule`,
  `ILintToolCommandHandler<TRequest, TResponse>`, `CliEnvelope<TResult>`,
  `CliError`, `CliDiagnostic`, `CliErrorKind`, `LintToolRequest`,
  `LintToolResult`, `LintFinding`, `ViewRequest`, `ViewResult`,
  `CheckRequest`, `CheckResult`, `ClippyRequest`, `ClippyResult`,
  `CiRequest`, `CiResult`, `VersionResult`

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
- identify which abstractions and contract items should be escalated into the
  later Phase C boundary-package planning sprint, but do not treat that
  planning work as part of B2 closeout

## Acceptance Criteria

- `docs/phase-B/cli-dogfood-findings.md` exists and covers the dogfooded CLI
  workflows
- `docs/phase-B/cli-dogfood-remediation-policy.md` exists and classifies every
  B2 finding disposition
- `docs/phase-B/cli-follow-up-issues.md` exists and contains every known CLI
  predictability or contract gap found during B2
- the sprint records which abstractions and contract types should later be
  brought under Phase C boundary-package planning

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
