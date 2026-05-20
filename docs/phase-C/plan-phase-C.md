# Phase C Plan

## 1. Goal

Phase C creates `sc-lint-roslyn-boundary` as a zero-gap product line, using one
fundamental deliverable per sprint and preserving operator-facing parity with
`sc-lint-boundary` wherever Roslyn-specific constraints do not force a
difference.

## 2. Deliverables

- a formal Phase C sprint plan
- a documented boundary-package delivery sequence with one fundamental
  deliverable per sprint
- a boundary-package command/config parity baseline relative to
  `sc-lint-boundary`
- a graph-export plan tied to the approved shared schema
- a fully implemented `sc-lint-roslyn-boundary` delivery line with no planned
  gaps inside completed sprints

## 3. Execution Branch

- branch: `integration/phase-C`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/phase-B/plan-phase-B.md`
- `docs/phase-B/sprint-B1.md`
- `docs/phase-B/sprint-B2.md`
- `docs/phase-B/sprint-B3.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-toml-migration.md`
- `/Users/randlee/Documents/github/atm-core/docs/sc-lint/boundary-enforcement-model.md`

## 5. Exact Implementation Targets

- `docs/phase-C/boundary-package-plan.md`
- `docs/phase-C/boundary-package-deep-dive.md`
- `docs/phase-C/graph-schema-blockers.md`
- `docs/phase-C/sprint-C1.md`
- `docs/phase-C/sprint-C2.md`
- `docs/phase-C/sprint-C3.md`
- `docs/phase-C/sprint-C4.md`
- `docs/phase-C/sprint-C5.md`
- `docs/phase-C/sprint-C6.md`
- `docs/phase-C/sprint-C7.md`
- `docs/phase-C/sprint-C8.md`
- `docs/phase-C/sprint-C9.md`
- `src/sc.lint.roslyn.boundary/`
- `docs/sc-lint-roslyn-boundary/`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| C1 | Boundary package planning | Finalize the `sc-lint-roslyn-boundary` delivery sequence, command/config parity targets, and schema constraints |
| C2 | Boundary package scaffold | Create the package and top-level command shell only |
| C3 | Boundary config format and loader | Deliver the canonical machine-readable boundary config and loader only |
| C4 | Roslyn graph extraction | Deliver Roslyn graph extraction only |
| C5 | Graph export schema | Deliver graph export using the approved schema only |
| C6 | Boundary inventory rule family | Deliver the first boundary inventory/parity rule family only |
| C7 | Planned-gap escalation | Deliver structured planning metadata and warn/error escalation only |
| C8 | Boundary package dogfooding | Run `sc-lint-roslyn-boundary` on this repo and capture findings only |
| C9 | Boundary package CI and release hardening | Deliver CI, packaging, and release hardening only |

## 7. Implementation Strategy

- Phase C reuses `sc-lint-boundary` and `atm-core` as the primary planning
  references for guard rails, config shape, and operator workflow
- every completed Phase C sprint must own exactly one fundamental deliverable
  and may not close with known planned gaps for that deliverable
- command and config parity should stay as close to `sc-lint-boundary` as
  practical
- graph-export planning remains blocked from finalization until maintainer
  schema details are provided
- graph extraction and graph export remain separate sprints so schema drift is
  not hidden inside extraction work

## 8. Acceptance

Phase C planning is established only when:

- `docs/phase-C/plan-phase-C.md` exists
- every planned sprint from `C1` through `C9` has its own full sprint doc
- `docs/phase-C/boundary-package-plan.md` and
  `docs/phase-C/boundary-package-deep-dive.md` exist
- the graph-schema blocker is documented explicitly in
  `docs/phase-C/graph-schema-blockers.md`
- the plan shows one fundamental deliverable per sprint with no mixed-scope
  closeout
