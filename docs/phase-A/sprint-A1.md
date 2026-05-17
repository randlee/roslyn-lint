---
id: A1
title: Documentation reset
status: complete
branch: integration/phase-A
target: repo-docs
---

# Sprint A1 - Documentation Reset

## Goal

- Replace placeholder or unapproved repository docs with a formal suite-level
  and project-level documentation framework.
- Establish the replacement-first rule for noncompliant spike code.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `../atm-core/docs/documentation-guidelines.md`
- `.claude/skills/codex-orchestration/sprint-plan.md.j2`

## Exact Targets

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/documentation-guidelines.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/phase-A/plan-phase-A.md`
- `docs/phase-A/sprint-A1.md`
- `docs/phase-A/sprint-A2.md`
- `docs/phase-A/sprint-A3.md`
- `docs/phase-A/sprint-A4.md`

## Required Work

- write top-level suite requirements, architecture, and project plan
- add project-level requirements, architecture, and boundary docs for
  `Roslyn.DeMagic` and `roslyn-lint`
- add a formal Phase A plan and sprint set using the standard sprint shape
- make current code explicitly subordinate to approved documents
- make replacement of noncompliant spike code an explicit rule

## Acceptance Criteria

- documentation structure matches the `atm-core` pattern at the repo level
- project docs exist for both product surfaces
- boundary inventories exist for both product surfaces
- current code is explicitly treated as subordinate to approved docs
- sprint docs include branch, target, exact targets, and validation sections

## Required Validation

- `git diff --check`
