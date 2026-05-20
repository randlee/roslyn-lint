---
id: C6
title: boundary inventory rule family
status: planned
branch: sprint/C6
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C6
target: integration/phase-C
---

# Sprint C6 - Boundary Inventory Rule Family

## Goal

- Deliver the first boundary inventory/parity rule family only.

## Hard Dependencies

- `docs/phase-C/sprint-C3.md`
- `docs/phase-C/sprint-C4.md`
- `docs/phase-C/sprint-C5.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`

## Exact Targets

- `src/sc.lint.roslyn.boundary/Rules/`
- `src/sc.lint.roslyn.boundary/Rules/InventoryParityAnalyzer.cs`
- `src/sc.lint.roslyn.boundary/Rules/BoundaryFinding.cs`
- `tests/sc.lint.roslyn.boundary.tests/Rules/`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- implement the first boundary inventory/parity rule family only
- define the first exact item classes in parity scope
- define the first stable rule ids for missing documented items
- add tests for satisfied items and missing/unplanned items
- do not implement planned-gap warning eligibility or overdue escalation in this
  sprint

## Acceptance Criteria

- the first inventory/parity rule family is implemented and documented
- satisfied items and missing/unplanned items are tested
- planned-gap warning logic is still absent from this sprint
- no CI/release hardening is mixed into this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
