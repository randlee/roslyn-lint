---
id: C8
title: boundary package dogfooding
status: planned
branch: sprint/C8
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C8
target: integration/phase-C
---

# Sprint C8 - Boundary Package Dogfooding

## Goal

- Run `sc-lint-roslyn-boundary` on this repository and capture findings only.

## Hard Dependencies

- `docs/phase-C/sprint-C6.md`
- `docs/phase-C/sprint-C7.md`
- `docs/phase-C/boundary-package-plan.md`
- `docs/phase-C/boundary-package-deep-dive.md`

## Exact Targets

- `docs/phase-C/boundary-dogfood-findings.md`
- `docs/phase-C/boundary-dogfood-remediation-policy.md`
- `docs/phase-C/boundary-follow-up-issues.md`
- `src/sc.lint.roslyn.boundary/`
- `tests/sc.lint.roslyn.boundary.tests/`

## Required Work

- run `sc-lint-roslyn-boundary` on repository-owned code and boundary records
- record every finding in `docs/phase-C/boundary-dogfood-findings.md`
- classify every finding in `docs/phase-C/boundary-dogfood-remediation-policy.md`
- file every predictability, correctness, or ownership gap in
  `docs/phase-C/boundary-follow-up-issues.md`
- keep the sprint focused on dogfooding and findings capture rather than adding
  new rule families

## Acceptance Criteria

- the boundary package runs on this repository
- boundary dogfood findings are recorded explicitly
- every finding has a remediation classification
- every correctness or predictability gap has a follow-up issue entry
- the sprint does not absorb CI/release hardening work

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
