---
id: phase-A
title: Formal docs framework and Roslyn.DeMagic v1
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Phase A — Formal Docs Framework And `Roslyn.DeMagic` v1

## Goal

Establish the `atm-core`-style documentation framework for this repository and
deliver the first production-ready `Roslyn.DeMagic` implementation line.

## Current Baseline

The repository already contains:
- solution and package scaffolding
- analyzer and CLI spike implementations
- basic tests
- CI and publish workflows

The repository does not yet have:
- formal repo-level requirements, architecture, and project planning
- per-project requirements and architecture docs
- a documented decision on which current code is authoritative versus disposable
- a production-confirmed implementation of the PRD-defined `DM001` and `DM002`
  contracts

## Phase Deliverables

- repo-level docs:
  - `docs/requirements.md`
  - `docs/architecture.md`
  - `docs/project-plan.md`
- project-level docs:
  - `docs/roslyn-demagic/requirements.md`
  - `docs/roslyn-demagic/architecture.md`
  - `docs/roslyn-lint/requirements.md`
  - `docs/roslyn-lint/architecture.md`
- sprint docs for concrete implementation sequencing
- code aligned to the documented target, with deletion or replacement of
  conflicting spike code where necessary

## Sprint Sequence

- [`sprint-A1.md`](./sprint-A1.md) — docs framework and baseline disposition
- [`sprint-A2.md`](./sprint-A2.md) — config model and `DM001`
- [`sprint-A3.md`](./sprint-A3.md) — `DM002` and analyzer contract alignment
- [`sprint-A4.md`](./sprint-A4.md) — CLI alignment, packaging, and release gate

## Acceptance Criteria

- all required docs exist and agree on project ownership
- current code disposition is explicit: keep, refactor, replace, or delete
- `Roslyn.DeMagic` matches the documented rule and config contract
- `roslyn-lint` either faithfully represents the analyzer contract or is
  narrowed until it does
- CI, tests, and packaging support the documented delivery line
