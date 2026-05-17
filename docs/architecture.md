# Roslyn Lint Suite Architecture

## 1. Overview

The suite currently has two product boundaries:

- `Roslyn.DeMagic` owns Roslyn diagnostic analysis and analyzer configuration
- `roslyn-lint` owns CLI command parsing, contract serialization, and future
  repo-facing operations

The repository architecture is intentionally split so the analyzer can mature
as a NuGet package without being blocked by the CLI feature backlog.

## 2. Project Boundaries

Architectural rules:

- `Roslyn.DeMagic` must remain usable as a standalone analyzer package with no
  dependency on the CLI executable at runtime.
- `roslyn-lint` may reference analyzer or shared operation assemblies, but it
  must not become the only way to consume `Roslyn.DeMagic`.
- Analyzer configuration ownership lives with the analyzer project, not the
  CLI presentation layer.
- CLI presentation and transport concerns must not leak into analyzer rule
  logic.

## 3. Documentation and Change Control

The architecture source of truth is document-first:

- top-level suite architecture lives here
- project-local architecture detail lives in `docs/<project>/architecture.md`
- phase sequencing lives in `docs/project-plan.md` and `docs/phase-A/`

Current code in `src/` is subordinate to these documents until it satisfies
them. Existing implementation spikes may be removed when they conflict with the
approved architecture.

Accepted repository ADRs live in `docs/adr/` and are indexed by
`docs/adr/INDEX.md`.

## 4. Phase A Architecture Direction

Phase A keeps the suite split into:

- an analyzer-first implementation line for `Roslyn.DeMagic`
- a contract-first design line for `roslyn-lint`

Architectural rules:

- the analyzer is the first production deliverable
- the current `DM001` and `DM002` implementations are not architectural proof
  that the PRD is satisfied
- the current Spectre-based CLI spike is not the approved `roslyn-lint`
  architecture baseline

## 5. CLI Architecture Baseline

The CLI architecture must follow an AI-first shape:

- request parsing
- request DTOs
- operation layer
- response DTOs
- JSON serialization
- human-readable formatting as a separate presentation concern

Architectural rules:

- every command must support `--json`
- JSON output is the normative contract
- success and failure responses must remain in one stable envelope family
- error results must be typed, structured, and actionable
- request and response DTOs must be reusable by a future MCP wrapper without
  reshaping business payloads
- mutating commands must have readback paths
- any external integration must sit behind a swappable adapter boundary so
  simulator-backed testing is possible

Project-level detail for those rules is defined in
`docs/roslyn-lint/architecture.md`.

Project-local boundary detail is owned by:

- `docs/roslyn-demagic/boundaries.md`
- `docs/roslyn-lint/boundaries.md`
