# ADR-001 — Analyzer-First Phase A And Replacement-First Spike Policy

| Field | Value |
|---|---|
| ID | ADR-001 |
| Status | **Accepted** |
| Date | 2026-05-17 |
| Deciders | Rand Lee |
| Relates to | REQ-SUITE-PRODUCT-003, REQ-SUITE-PHASEA-001, REQ-CLI-DOTNET-005 |
| Supersedes | — |

---

## Context

The repository contains an initial analyzer and CLI spike, but the user has
explicitly rejected any assumption that this code is correct or should be
preserved.

Phase A also has an asymmetric priority:

- `Roslyn.DeMagic` is the first implementation line that must become
  production-correct
- the CLI must keep a strict design baseline, but detailed CLI product
  requirements will arrive later

Without an explicit decision, planning and implementation could drift toward
incrementally editing misleading spike structures only because they already
exist.

## Decision Drivers

- implementation must follow approved requirements and architecture, not the
  existing spike
- misleading type names and file shapes create review drag and preserve the
  wrong semantics
- the analyzer needs a clean compliance path to the PRD
- future CLI work must not inherit a human-first spike structure as the default

## Decision

Phase A is analyzer-first, and noncompliant spike code is replacement-first.

This means:

- `Roslyn.DeMagic` implementation work takes priority over speculative CLI
  feature expansion
- if a spike file, class, or test encodes rejected behavior or misleading
  naming, deletion and replacement is preferred over preserving it
- compatibility with spike-era code structure is not a goal by itself
- implementation reviews should reject plans that keep misleading spike
  semantics solely to avoid file churn

## Consequences

### Positive

- implementation can converge directly on approved requirements
- analyzer code can use semantically accurate types and file names
- CLI planning remains strict without pretending the current command structure
  is acceptable

### Negative

- there may be higher short-term file churn than in a patch-in-place approach
- some spike-era tests and type names will be discarded instead of evolved

## Follow-Up Work

- keep Phase A sprint plans explicit about replacement-first execution
- remove spike-era analyzer and CLI structures when they obstruct compliance
- ensure replacement work keeps the solution compiling and tests runnable
