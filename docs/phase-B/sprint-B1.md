---
id: B1
title: local demagic analyzer dogfooding
status: planned
branch: sprint/B1
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B1
target: integration/phase-B
---

# Sprint B1 - Local DeMagic Analyzer Dogfooding

## Goal

- Start using `sc.lint.roslyn.demagic` on this repository's own projects in local
  development.
- Systematically locate constant-consolidation, forbidden-string, and
  deduplication issues across both product codebases.
- Prove whether analyzer behavior is fully predictable on real repository code.
- File follow-up issues for anything that does not work exactly as expected.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A12.md`
- `docs/phase-A/sprint-A13.md`
- `Directory.Build.props`
- `.sc-lint-roslyn/config-src.toml`
- `.sc-lint-roslyn/config-test.toml`

## Exact Targets

- `docs/phase-B/dogfood-findings.md`
- `docs/phase-B/dogfood-remediation-policy.md`
- `docs/phase-B/dogfood-follow-up-issues.md`
- `Directory.Build.props`
- `.sc-lint-roslyn/config-src.toml`
- `.sc-lint-roslyn/config-test.toml`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn.abstractions/sc.lint.roslyn.abstractions.csproj`
- `src/sc.lint.roslyn.demagic.lint/sc.lint.roslyn.demagic.lint.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj`
- `tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj`

## Important Interfaces, Records/Structs, And Enums

- no new product-runtime interfaces or records are required to begin B1
- if sprint tooling is added to persist findings, keep it outside analyzer
  runtime code and prefer simple documented inventory artifacts first

## Required Work

- add local analyzer-consumption wiring for repository-owned source and test
  projects so `sc.lint.roslyn.demagic` runs during normal local builds
- keep the initial dogfooding path local-development oriented; do not require a
  pack/publish/install loop for each iteration
- preserve separate config routing for source and test projects under
  `.sc-lint-roslyn/config-src.toml` and `.sc-lint-roslyn/config-test.toml`
- define the exact initial project scope:
  `src/sc.lint.roslyn.demagic`,
  `src/sc.lint.roslyn.abstractions`,
  `src/sc.lint.roslyn.demagic.lint`,
  `src/sc.lint.roslyn`,
  `tests/sc.lint.roslyn.demagic.tests`,
  `tests/sc.lint.roslyn.tests`
- exclude packaged-consumer smoke examples from B1 findings inventory; they
  remain a separate validation surface from Phase A
- record every finding emitted by the first dogfooding pass in
  `docs/phase-B/dogfood-findings.md`
- classify each finding in `docs/phase-B/dogfood-remediation-policy.md` as:
  immediate fix, suppression with rationale, config adjustment, or later-phase
  remediation
- create `docs/phase-B/dogfood-follow-up-issues.md` for analyzer behavior that
  is not fully predictable, produces incorrect diagnostics, misses expected
  consolidation/deduplication opportunities, or otherwise requires follow-up
- state the initial enforcement mode explicitly:
  B1 is non-blocking for merge while findings are being discovered and
  classified; later Phase B sprints may tighten that policy only after B1
  captures the real findings inventory

## Acceptance Criteria

- repository-owned source and test projects run `sc.lint.roslyn.demagic` during
  local builds
- source and test projects still route to distinct analyzer config files
- `docs/phase-B/dogfood-findings.md` exists and covers every in-scope project
- `docs/phase-B/dogfood-remediation-policy.md` exists and classifies every B1
  finding disposition
- `docs/phase-B/dogfood-follow-up-issues.md` exists and contains every known
  predictability or expectation gap found during B1
- the sprint outcome states explicitly that B1 is discovery and classification
  work, not immediate full enforcement

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
