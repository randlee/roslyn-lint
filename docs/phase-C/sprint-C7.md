---
id: C7
title: planned-gap escalation
status: planned
branch: sprint/C7
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C7
target: integration/phase-C
---

# Sprint C7 - Planned-Gap Escalation

## Goal

- Deliver structured planning metadata and warn/error escalation only.

## Hard Dependencies

- `docs/phase-C/sprint-C3.md`
- `docs/phase-C/sprint-C6.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`
- `/Users/randlee/Documents/github/atm-core/docs/sc-lint/boundary-enforcement-model.md`

## Exact Targets

- `boundaries/planning.toml`
- `src/sc.lint.roslyn.boundary/Planning/`
- `src/sc.lint.roslyn.boundary/Planning/PlannedItemMetadata.cs`
- `src/sc.lint.roslyn.boundary/Planning/SprintOrdering.cs`
- `tests/sc.lint.roslyn.boundary.tests/Planning/`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- implement structured planning metadata loading for planned boundary items
- implement current-sprint parsing and sprint ordering
- implement warning eligibility for future planned gaps
- implement automatic escalation for overdue planned gaps
- add tests for valid planned warnings, malformed metadata, missing metadata,
  and overdue escalation
- do not add new boundary rule families beyond the escalation behavior applied
  to the existing inventory rule family

## Acceptance Criteria

- planned-gap metadata is machine-readable and enforced
- future planned gaps warn deterministically
- overdue planned gaps escalate deterministically
- malformed planning metadata fails closed
- no new unrelated rule families are mixed into this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
