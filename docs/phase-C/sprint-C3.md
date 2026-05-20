---
id: C3
title: boundary config format and loader
status: planned
branch: sprint/C3
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C3
target: integration/phase-C
---

# Sprint C3 - Boundary Config Format And Loader

## Goal

- Deliver the canonical machine-readable boundary config format and loader only.

## Hard Dependencies

- `docs/phase-C/sprint-C2.md`
- `docs/phase-C/boundary-package-plan.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-toml-migration.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`

## Exact Targets

- `boundaries/`
- `boundaries/planning.toml`
- `src/sc.lint.roslyn.boundary/Configuration/`
- `src/sc.lint.roslyn.boundary/Configuration/BoundaryConfigLoader.cs`
- `src/sc.lint.roslyn.boundary/Configuration/BoundaryRecord.cs`
- `src/sc.lint.roslyn.boundary/Configuration/PlanningMetadata.cs`
- `tests/sc.lint.roslyn.boundary.tests/Configuration/`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- define the canonical machine-readable boundary record shape
- define the planning metadata shape for future planned-gap escalation
- implement discovery and loading for boundary records and planning metadata
- decide and document strict unknown-field behavior
- add tests for valid load, malformed records, malformed planning metadata, and
  duplicate authoritative definitions
- do not implement graph extraction, export, or rule evaluation in this sprint

## Acceptance Criteria

- canonical boundary config and planning metadata shapes are documented
- boundary records and planning metadata load deterministically
- malformed or conflicting records fail closed
- graph extraction, export, and rule logic are still absent from this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
