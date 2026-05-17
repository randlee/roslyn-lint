# Roslyn Lint Suite Requirements

## 1. Product Definition

The `roslyn-lint` repository owns a .NET linting suite with two distinct product
surfaces:

- `Roslyn.DeMagic`, a Roslyn analyzer package that enforces configurable code
  hygiene rules inside consuming C# projects
- `roslyn-lint`, a command-line tool that will expose analyzer and repository
  operations through an AI-first machine contract

Suite requirement IDs:

- `REQ-SUITE-PRODUCT-001` The repository must preserve separate product
  ownership for the analyzer package and the CLI tool.
- `REQ-SUITE-PRODUCT-002` Phase A implementation focus is `Roslyn.DeMagic`.
- `REQ-SUITE-PRODUCT-003` Existing code or documentation that conflicts with
  approved requirements is disposable and may be deleted rather than preserved
  for compatibility with an unapproved spike.

The current analyzer and CLI code are implementation probes, not source-of-
truth product contracts.

## 2. Documentation Structure

Top-level suite documents in `docs/` are the product source of truth:

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/adr/INDEX.md`

Project-level ownership docs live under:

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/boundaries.md`

Documentation structure and ownership rules are defined in:

- `docs/documentation-guidelines.md`

Phase planning docs live under:

- `docs/phase-A/plan-phase-A.md`
- `docs/phase-A/sprint-A1.md`
- `docs/phase-A/sprint-A2.md`
- `docs/phase-A/sprint-A3.md`
- `docs/phase-A/sprint-A4.md`

Documentation requirement IDs:

- `REQ-SUITE-DOCS-001` Approved requirements and architecture decisions must be
  written into the repo before implementation is treated as authoritative.
- `REQ-SUITE-DOCS-002` Project-local documents must not contradict top-level
  suite requirements.
- `REQ-SUITE-DOCS-003` Cross-cutting enforceable decisions must be recorded as
  ADRs in `docs/adr/`.

## 3. Scope

In-scope product areas:

- analyzer packages published from this repository
- CLI tooling published from this repository
- analyzer configuration conventions
- test and release validation for both surfaces
- repo-level planning and phase tracking

Out of scope for Phase A:

- code fix providers
- non-C# language targets
- a finalized full CLI feature set beyond the approved contract baseline

## 4. Phase A Priority

Phase A is an analyzer-first line.

Suite requirement IDs:

- `REQ-SUITE-PHASEA-001` Phase A must deliver a requirements-compliant
  `Roslyn.DeMagic` analyzer before the repository treats the CLI as a primary
  implementation line.
- `REQ-SUITE-PHASEA-002` The analyzer design baseline is the PRD at
  `docs/prd/roslyn-demagic-prd.md`.
- `REQ-SUITE-PHASEA-003` The CLI remains a separate project in Phase A, but its
  design baseline is governed by AI-first contract rules rather than the
  current CLI spike.

## 5. CLI Baseline

The final detailed CLI feature requirements will be provided later, but the
contract model is not open-ended.

Suite requirement IDs:

- `REQ-SUITE-CLI-001` All CLI work must follow an AI-first contract with
  machine output primary and human formatting secondary.
- `REQ-SUITE-CLI-002` Every CLI command must support `--json`.
- `REQ-SUITE-CLI-003` Success and failure results must use one stable JSON
  contract family with typed, actionable errors.
- `REQ-SUITE-CLI-004` CLI request and response DTOs must remain reusable for a
  future MCP wrapper with no business-payload reshaping.
- `REQ-SUITE-CLI-005` Mutating CLI commands must have corresponding readback
  commands so state changes are auditable.

Project-level detail for those CLI obligations is defined in
`docs/roslyn-lint/requirements.md`.
