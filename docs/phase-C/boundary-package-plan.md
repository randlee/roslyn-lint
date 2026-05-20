# Boundary Package Plan

## Goal

Define the delivery sequence for `sc-lint-roslyn-boundary` as a zero-gap
product line.

Planning rule:

- every boundary-package sprint owns exactly one fundamental deliverable
- no sprint closes with known planned implementation gaps
- if a feature is not fully implemented, it belongs to a later sprint rather
  than being deferred inside a "complete" sprint

## Planned Sprint Sequence

| Sprint | Fundamental deliverable |
| --- | --- |
| C1 | boundary-package planning finalized |
| C2 | package scaffold and top-level command shell |
| C3 | machine-readable boundary config format and loader |
| C4 | Roslyn graph extraction |
| C5 | graph export on the approved schema |
| C6 | first boundary inventory/parity rule family |
| C7 | structured planning metadata and warn/error escalation |
| C8 | repo-local boundary-package dogfooding |
| C9 | CI, packaging, and release hardening |

## Command And Config Parity Direction

Default direction:

- mirror `sc-lint-boundary` command concepts where practical
- mirror machine-readable config concepts where practical
- keep language-specific differences confined to graph extraction and rule
  semantics rather than operator workflow

Planned parity targets:

- similar top-level analyze/export command model
- similar machine-readable boundary source layout
- similar planned-gap and inventory-parity concepts
- same graph-export schema, pending maintainer-provided unified schema details

## Finalization Blockers

- graph export schema is not final until the maintainer provides the unified
  cross-language schema details
- any C5 planning that assumes field-level schema shape before that input is
  provisional only
