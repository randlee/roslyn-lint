# roslyn-lint Project Plan

## 1. Goal

Phase A establishes the formal documentation and implementation baseline for
the `roslyn-lint` suite and delivers the first production-ready
`Roslyn.DeMagic` line.

The current repository implementation is treated as a spike baseline only.
Anything that does not satisfy the PRD or the formal docs may be deleted or
replaced.

## 1.1 Documentation Structure

Top-level product docs:
- [`requirements.md`](./requirements.md)
- [`architecture.md`](./architecture.md)
- [`project-plan.md`](./project-plan.md)

Project-level docs:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)
- [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)
- [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)

Active phase planning:
- [`docs/phase-A/plan-phase-A.md`](./phase-A/plan-phase-A.md)

## 2. Deliverables

Phase A must deliver:
- formal repo-level requirements, architecture, and project-plan docs
- per-project requirements and architecture docs for `Roslyn.DeMagic` and
  `roslyn-lint`
- a documented phase-A plan with sprint-level execution breakdown
- a `Roslyn.DeMagic` implementation aligned to the PRD
- a CLI line that is either aligned to the analyzer contract or trimmed until
  it is
- test, CI, packaging, and release gates that prove the documented behavior

## 3. Projects

The active production projects are:
- `src/Roslyn.DeMagic`
- `src/Roslyn.Lint`

The active test projects are:
- `tests/Roslyn.DeMagic.Tests`
- `tests/Roslyn.Lint.Tests`

Ownership detail is defined in:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)
- [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)
- [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)

## 4. Current Baseline Assessment

Current repository state:
- package and solution scaffolding exist
- CI and publish workflows exist
- analyzer and CLI spike implementations exist
- tests exist for the current spike behavior

Current baseline risks:
- current analyzer behavior does not yet match the formal PRD
- current CLI execution path is more of a local analysis spike than a proven
  product architecture
- current docs were placeholders before Phase A planning

Disposition rule:
- no current code path is preserved merely because it exists
- code is retained only if it satisfies the documented requirements and
  architecture

## 5. Work Sequence

### Phase A: Formalize And Deliver `Roslyn.DeMagic` v1 [ACTIVE]

Status summary:
- worktree: `integration/phase-A`
- target branch: `develop`
- active scope: document framework, product contract, project-local ownership,
  and first production implementation line

Phase-A deliverables are planned in:
- [`docs/phase-A/plan-phase-A.md`](./phase-A/plan-phase-A.md)

Acceptance gates:
- repo-level requirements, architecture, and plan are complete
- per-project requirements and architecture docs exist
- phase-A sprint docs exist and define concrete ownership
- `Roslyn.DeMagic` behavior matches documented rule and config contracts
- CLI behavior is aligned with the documented architecture
- CI, tests, and packaging validate the final line

### Future Phases

Future phases are out of scope until Phase A lands. Once Phase A is complete,
the project plan should be extended with the next analyzer or tooling phase
rather than overloading this document with speculative future work.
