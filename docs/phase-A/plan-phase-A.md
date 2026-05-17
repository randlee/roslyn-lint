<<<<<<< HEAD
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
Detailed CLI product work is deferred.

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
- [`sprint-A4.md`](./sprint-A4.md) — analyzer packaging, release gate, and CLI deferral boundary

## Acceptance Criteria

- all required docs exist and agree on project ownership
- current code disposition is explicit: keep, refactor, replace, or delete
- `Roslyn.DeMagic` matches the documented rule and config contract
- `roslyn-lint` remains documented as a separate project without hijacking the
  analyzer contract before later CLI requirements are written
- CI, tests, and packaging support the documented delivery line
=======
# Phase A Plan

## 1. Goal

Phase A establishes the repository's formal documentation baseline and delivers
the first approved implementation line for `Roslyn.DeMagic`.

The CLI is still part of the suite, but Phase A treats it as a contract-first
design line rather than assuming the current spike is valid.

## 2. Deliverables

- suite-level documentation framework
- project-level requirements and architecture for both products
- project-level boundary inventories for both products
- PRD-aligned `Roslyn.DeMagic` design and implementation plan
- AI-first `roslyn-lint` CLI baseline
- packaging and validation gates for the analyzer

## 3. Execution Branch

- branch: `integration/phase-A`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/roslyn-demagic/*`
- `docs/roslyn-lint/*`
- `.claude/skills/creating-ai-clis/`

## 5. Exact Implementation Targets

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| A1 | Documentation reset | Replace placeholders and unapproved assumptions with approved suite and project docs |
| A2 | `DM001` requirements convergence | Replace the numeric-literal spike with config-driven constant-consolidation analysis |
| A3 | `DM002` and analyzer hardening | Replace the generic string-literal spike with forbidden-pattern analysis and aligned release metadata |
| A4 | Packaging and CLI baseline correction | Finalize analyzer package and release gates and define the replacement-oriented CLI baseline |

## 7. Implementation Strategy

- A1 documents the approved target and replacement policy
- A2 deletes the `DM001` spike behavior and introduces the real config and
  declaration-analysis path
- A3 deletes the `DM002` spike behavior and introduces compiled forbidden-
  pattern matching and analyzer metadata alignment
- A4 validates analyzer package outputs and leaves the CLI with a strict design
  baseline that future implementation must follow
- no sprint in Phase A should preserve current spike semantics merely because
  code already exists

## 8. Acceptance

Phase A is complete only when:

- the repo documentation framework exists and is internally consistent
- `Roslyn.DeMagic` behavior matches the PRD rather than the current spike
- analyzer packaging and tests reflect the approved diagnostic set
- the CLI baseline no longer treats the current implementation as an approved
  design
- the Phase A execution rules explicitly prefer deleting and replacing
  noncompliant spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets and validation commands to drive
  implementation directly
>>>>>>> f9fe54d (Finalize phase A planning framework)
