---
id: B4
title: phase b closeout hardening
status: complete
branch: sprint/B4
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/B4
target: integration/phase-B
---

# Sprint B4 - Phase B Closeout Hardening

## Goal

- close the remaining review-quality gaps from the Phase B extraction-readiness
  review
- make the integrated Phase B docs read as complete, reproducible, and ready
  to hand off into Phase C planning

## Hard Dependencies

- `docs/phase-B/plan-phase-B.md`
- `docs/phase-B/sprint-B1.md`
- `docs/phase-B/sprint-B2.md`
- `docs/phase-B/sprint-B3.md`
- `docs/phase-B/issues-inventory.md`
- `docs/phase-B/dogfood-findings.md`
- `docs/phase-B/cli-dogfood-findings.md`
- `docs/phase-B/package-doc-findings.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/project-plan.md`

## Exact Targets

- `docs/phase-B/issues-inventory.md`
- `docs/phase-B/dogfood-findings.md`
- `docs/phase-B/cli-dogfood-findings.md`
- `docs/phase-B/package-doc-findings.md`
- `docs/phase-B/package-doc-follow-up-issues.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/phase-B/plan-phase-B.md`
- `docs/project-plan.md`
- `docs/phase-B/sprint-B4.md`

## Deliverables

- Phase B inventory status reflects the integrated closeout state
- B1 findings include explicit severity and an exact warm-build reproduction
  note
- B2 findings include explicit severity
- B3 findings include explicit severity
- B3 follow-up issues use a structurally explicit `none` record
- the CLI contract reads as a current-state contract document rather than a
  mixed current-plus-rollout planning artifact
- Phase B planning and project indexes include the B4 closeout-hardening sprint

## Required Work

- update `docs/phase-B/issues-inventory.md` so the integrated branch no longer
  reports `B1`, `B2`, and `B3` as planned
- add a severity column to the B1, B2, and B3 findings tables
- document the exact warm-build self-analysis sequence for B1
- tighten `docs/sc-lint-roslyn/cli-contract.md` into current-state contract
  language and remove sprint-rollout table wording
- make the B3 follow-up artifact structurally explicit even when there are no
  open follow-up issues
- add B4 to the Phase B planning and project-plan indexes

## Non-Closure Items

- B4 does not change product runtime code
- B4 does not add new analyzer rules, CLI families, or Phase C implementation
- B4 does not reopen B1, B2, or B3 scope beyond documentation hardening

## Acceptance Criteria

- `docs/phase-B/issues-inventory.md` shows `B1`, `B2`, `B3`, and `B4` as
  complete
- `docs/phase-B/dogfood-findings.md` includes both severity and exact warm-build
  reproduction steps
- `docs/sc-lint-roslyn/cli-contract.md` contains no `Planned` section headers
  or sprint rollout membership table wording
- `docs/phase-B/cli-dogfood-findings.md` includes severity
- `docs/phase-B/package-doc-findings.md` includes severity
- `docs/phase-B/package-doc-follow-up-issues.md` uses an explicit `none` row
- `docs/project-plan.md` includes the B4 sprint entry

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
