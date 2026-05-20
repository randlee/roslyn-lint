---
id: C5
title: graph export schema
status: planned
branch: sprint/C5
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C5
target: integration/phase-C
---

# Sprint C5 - Graph Export Schema

## Goal

- Deliver graph export using the approved schema only.

## Hard Dependencies

- `docs/phase-C/sprint-C4.md`
- `docs/phase-C/graph-schema-blockers.md`
- `docs/phase-C/boundary-package-plan.md`
- maintainer-provided unified cross-language graph schema details

## Exact Targets

- `src/sc.lint.roslyn.boundary/Export/`
- `src/sc.lint.roslyn.boundary/Export/GraphExporter.cs`
- `src/sc.lint.roslyn.boundary/Export/GraphJsonContext.cs`
- `tests/sc.lint.roslyn.boundary.tests/Export/`
- `docs/sc-lint-roslyn-boundary/architecture.md`
- `docs/phase-C/graph-schema-blockers.md`

## Required Work

- map the internal Roslyn graph model onto the approved export schema
- keep export semantics aligned with `sc-lint-boundary` until the new unified
  schema is provided
- once the unified schema is provided, adopt it as the canonical export
  contract
- add tests for schema shape, stable field naming, and representative export
  payloads
- do not implement boundary rule evaluation or planned-gap escalation in this
  sprint

## Acceptance Criteria

- graph export matches the approved schema
- schema fixtures and tests cover representative export cases
- the sprint is blocked from closeout if maintainer-provided schema details are
  still missing
- no rule evaluation or planning-metadata escalation work is mixed into this
  sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
