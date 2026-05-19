---
id: A0
title: Documentation reset
status: complete
branch: integration/phase-A
target: repo-docs
---

# Sprint A0 - Documentation Reset

## Goal

- Replace placeholder or unapproved repository docs with a formal suite-level
  and project-level documentation framework.
- Establish the replacement-first rule for noncompliant spike code.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `../atm-core/docs/documentation-guidelines.md`
- `.claude/skills/codex-orchestration/sprint-plan.md.j2`

## Exact Targets

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/documentation-guidelines.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/sc-lint-roslyn/boundaries.md`
- `docs/phase-A/plan-phase-A.md`
- `docs/phase-A/sprint-A0.md`
- `docs/phase-A/sprint-A1.md`
- `docs/phase-A/sprint-A2.md`
- `docs/phase-A/sprint-A3.md`
- `docs/phase-A/sprint-A4.md`
- `docs/phase-A/sprint-A5.md`
- `docs/phase-A/sprint-A6.md`
- `docs/phase-A/sprint-A7.md`
- `docs/phase-A/sprint-A8.md`

## Required Work

- write top-level suite requirements, architecture, and project plan
- add project-level requirements, architecture, contract, and boundary docs
  for `sc.lint.roslyn.demagic` and `sc-lint-roslyn`
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
