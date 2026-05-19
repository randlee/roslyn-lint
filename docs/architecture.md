# sc-lint-roslyn Suite Architecture

## 1. Overview

The suite currently has two product boundaries:

- `sc.lint.roslyn.demagic` owns Roslyn diagnostic analysis and analyzer configuration
- `sc-lint-roslyn` owns CLI command parsing, contract serialization, and future
  repo-facing operations across package-owned lint tools

The repository architecture is intentionally split so the analyzer can mature
as a NuGet package without being blocked by the CLI feature backlog.

## 2. Project Boundaries

Architectural rules:

- `sc.lint.roslyn.demagic` must remain usable as a standalone analyzer package with no
  dependency on the CLI executable at runtime.
- `sc-lint-roslyn` may reference analyzer or shared operation assemblies, but it
  must not become the only way to consume `sc.lint.roslyn.demagic`.
- `sc-lint-roslyn` is the stable orchestration surface for suite tools; backend
  packages may be linked in-process or invoked out-of-process without changing
  the public command contract.
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

- an analyzer-first implementation line for `sc.lint.roslyn.demagic`
- a contract-first design line for `sc-lint-roslyn`

Architectural rules:

- the analyzer is the first production deliverable
- the current `DM001` and `DM002` implementations are not architectural proof
  that the PRD is satisfied
- the current Spectre-based CLI spike is not the approved `sc-lint-roslyn`
  architecture baseline

## 5. CLI Architecture Baseline

The CLI architecture must follow an AI-first shape:

- request parsing
- command-family and tool-target resolution
- request DTOs
- backend dispatch or in-process execution
- top-level result normalization
- operation layer
- response DTOs
- JSON serialization
- human-readable formatting as a separate presentation concern

Architectural rules:

- every command must support `--json`
- JSON output is the normative contract
- success and failure responses must remain in one stable envelope family
- the stable envelope family for `sc-lint-roslyn` must align with the `sc-lint`
  product pattern: `ok`, `command`, `data`, `error`, and optional
  `diagnostics`
- error results must be typed, structured, and actionable
- request and response DTOs must be reusable by a future MCP wrapper without
  reshaping business payloads
- mutating commands must have readback paths
- any external integration must sit behind a swappable adapter boundary so
  simulator-backed testing is possible

Project-level detail for those rules is defined in
`docs/sc-lint-roslyn/architecture.md` and `docs/sc-lint-roslyn/cli-contract.md`.

Project-local boundary detail is owned by:

- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/sc-lint-roslyn/boundaries.md`
