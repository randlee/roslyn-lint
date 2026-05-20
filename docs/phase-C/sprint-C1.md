---
id: C1
title: boundary package planning
status: planned
branch: sprint/C1
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C1
target: integration/phase-C
---

# Sprint C1 - Boundary Package Planning

## Goal

- Finalize the `sc-lint-roslyn-boundary` product plan before implementation.
- Lock the command and config parity direction relative to `sc-lint-boundary`.
- Decompose the boundary-package line into one-fundamental-deliverable sprints
  with zero planned implementation gaps per sprint.

## Hard Dependencies

- `docs/phase-C/boundary-package-deep-dive.md`
- `docs/phase-C/graph-schema-blockers.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-toml-migration.md`
- `/Users/randlee/Documents/github/atm-core/docs/sc-lint/boundary-enforcement-model.md`

## Exact Targets

- `docs/phase-C/boundary-package-plan.md`
- `docs/phase-C/sprint-C1.md`
- `docs/phase-C/sprint-C2.md`
- `docs/phase-C/sprint-C3.md`
- `docs/phase-C/sprint-C4.md`
- `docs/phase-C/sprint-C5.md`
- `docs/phase-C/sprint-C6.md`
- `docs/phase-C/sprint-C7.md`
- `docs/phase-C/sprint-C8.md`
- `docs/phase-C/sprint-C9.md`
- `docs/phase-C/graph-schema-blockers.md`
- `docs/project-plan.md`
- `docs/phase-C/plan-phase-C.md`

## Required Work

- finalize the boundary-package sprint sequence from C2 through C9
- define the default command-parity direction:
  `sc-lint-roslyn boundary analyze`, `export-graph`, and parallel machine
  output modes unless a Roslyn-specific constraint forces a change
- define the default config-parity direction:
  machine-readable boundary sources and planning metadata should mirror
  `sc-lint-boundary` unless a Roslyn-specific constraint forces a change
- record the package name, repo path, and documentation ownership baseline for
  `sc-lint-roslyn-boundary`
- record the graph-schema finalization blocker and make it explicit that C5
  cannot close without maintainer-provided schema details
- verify that every boundary-package sprint owns exactly one fundamental
  deliverable and has no hidden secondary scope

## Acceptance Criteria

- `docs/phase-C/boundary-package-plan.md` defines the full C1-C9 sequence
- every boundary-package sprint from C2-C9 has its own full sprint doc
- command/config parity direction is documented explicitly
- graph-schema finalization blocker is documented explicitly
- no boundary-package sprint still mixes multiple fundamental deliverables

## Required Validation

- `git diff --check`
