<<<<<<< HEAD
# roslyn-lint CLI Requirements

## 1. Purpose

This document defines the current `roslyn-lint` CLI project boundary at a high
level only.

Suite-level product behavior remains defined in
[`../requirements.md`](../requirements.md).

## 2. Ownership

`roslyn-lint` owns:
- command-line surface and argument parsing
- target path selection
- output formatting
- local orchestration of analyzer execution
- CLI-specific tests
- .NET tool packaging

`roslyn-lint` does not own:
- `DM001` or `DM002` rule semantics
- analyzer config schema design
- analyzer package layout

## 3. Requirement Namespace

The `roslyn-lint` CLI project uses the `REQ-CLI-*` namespace.

Detailed CLI feature requirements are intentionally deferred until a dedicated
CLI requirements pass is available. This document exists so the repository has
an explicit ownership boundary now instead of leaving the CLI as undocumented
drift.

## 4. Boundary Requirements

- `REQ-CLI-BOUNDARY-001` The CLI remains a separate project with its own
  command surface and packaging line.
- `REQ-CLI-BOUNDARY-002` The CLI must not become the source of truth for
  `Roslyn.DeMagic` rule semantics or analyzer config behavior.
- `REQ-CLI-BOUNDARY-003` Any current CLI behavior that conflicts with future
  dedicated CLI requirements may be refactored or removed.

## 5. Deferred Detail

The following are intentionally deferred:
- final command surface
- final analysis orchestration model
- final output contracts
- final exit-code policy
- final packaging and UX expectations beyond basic project separation

## 6. Packaging Requirements

- `REQ-CLI-PACKAGE-001` The CLI must remain packable as a .NET tool with
  command name `roslyn-lint`.
- `REQ-CLI-PACKAGE-002` CLI packaging must remain separate from analyzer
  packaging even when both ship from the same repository release.

## 7. Validation Requirements

- `REQ-CLI-TEST-001` Existing CLI tests may remain as provisional spike tests,
  but they are not the final product contract.
- `REQ-CLI-TEST-002` Future CLI requirements must replace or expand the current
  tests with contract-driven coverage.
=======
# roslyn-lint Requirements

## 1. Product Definition

`roslyn-lint` is the suite CLI project.

This document defines the approved CLI design baseline. It does not freeze the
final command inventory; that detailed feature set will be supplied later.

The current CLI implementation is an unapproved spike and may be deleted.

Product requirement IDs:

- `REQ-CLI-PRODUCT-001` `roslyn-lint` remains a first-class project in this
  repository distinct from `Roslyn.DeMagic`.
- `REQ-CLI-PRODUCT-002` The CLI must be designed for AI and system consumption
  first, with human-readable output as a secondary presentation layer.
- `REQ-CLI-PRODUCT-003` No current code or document in this repository is
  grandfathered if it conflicts with this baseline.

## 2. Core Contract

Contract requirement IDs:

- `REQ-CLI-CONTRACT-001` Every command must support `--json`.
- `REQ-CLI-CONTRACT-002` `--json` mode is the normative output contract.
- `REQ-CLI-CONTRACT-003` Success and failure results must use one stable JSON
  envelope family across command modes.
- `REQ-CLI-CONTRACT-004` Important machine data must not be available only in
  prose or colorized text output.
- `REQ-CLI-CONTRACT-005` The CLI must expose explicit request and response
  models for structured operations rather than building JSON from formatted
  strings.
- `REQ-CLI-CONTRACT-006` The baseline JSON envelope must include:
  `success`, `operation`, and exactly one of `result` or `error`. Optional
  `warnings` may be included on either path.
- `REQ-CLI-CONTRACT-007` Exit behavior must be deterministic: success exits
  zero, contract or operational failure exits non-zero, and `--json` mode must
  still emit the contract envelope on failure.
- `REQ-CLI-CONTRACT-008` Commands that accept structured input must support a
  reusable machine input path such as stdin JSON, `--input`, or `--file`.

## 3. Error Contract

Error requirement IDs:

- `REQ-CLI-ERROR-001` Errors in `--json` mode must be structured JSON, not
  prose-only stderr output.
- `REQ-CLI-ERROR-002` Error results must expose a stable machine code and a
  broader error kind or category.
- `REQ-CLI-ERROR-003` Error results must include actionable guidance when a
  caller can recover.
- `REQ-CLI-ERROR-004` Validation, not-found, invalid-state, dependency, and
  internal failures must remain distinguishable for automation.
- `REQ-CLI-ERROR-005` The baseline error object must include:
  `kind`, `code`, `message`, and may include `details` and
  `suggested_action`.

## 4. MCP-Ready Contract

Compatibility requirement IDs:

- `REQ-CLI-MCP-001` CLI request and response DTOs must be reusable by a future
  MCP wrapper without business-payload reshaping.
- `REQ-CLI-MCP-002` Human-readable formatting must be isolated from business
  payload construction.
- `REQ-CLI-MCP-003` Future MCP tests must be able to reuse CLI JSON fixtures.

## 5. Auditability

Auditability requirement IDs:

- `REQ-CLI-AUDIT-001` Every mutating command must have a corresponding read,
  list, show, or status command that can verify resulting state.
- `REQ-CLI-AUDIT-002` Mutating JSON responses must report enough state for
  deterministic automation, including target identity and resulting status.
- `REQ-CLI-AUDIT-003` Exit codes alone are insufficient proof of success for a
  mutating command.

## 6. External Integration Simulation

Simulation requirement IDs:

- `REQ-CLI-SIM-001` Any future external integration must sit behind a swappable
  adapter boundary.
- `REQ-CLI-SIM-002` External-integration tests must run against a stateful
  simulator rather than live-only infrastructure.
- `REQ-CLI-SIM-003` Simulator-backed tests must support read-after-write and
  negative-path behavior.

## 7. .NET Implementation Requirements

Implementation requirement IDs:

- `REQ-CLI-DOTNET-001` The CLI must use explicit DTO types for request and
  response payloads when structured contracts are involved.
- `REQ-CLI-DOTNET-002` JSON serialization settings must be shared across CLI
  and future MCP entrypoints.
- `REQ-CLI-DOTNET-003` The command layer must stay thin over a reusable
  operation layer.
- `REQ-CLI-DOTNET-004` Parser or presentation library choice must not force the
  business contract to depend on human-oriented formatting.
- `REQ-CLI-DOTNET-005` If the current Spectre-based command structure blocks
  compliance with this baseline, it must be replaced instead of preserved.

## 8. Validation Requirements

Validation requirement IDs:

- `REQ-CLI-TEST-001` Tests must assert `--json` machine contracts directly.
- `REQ-CLI-TEST-002` Tests must cover success and failure envelopes.
- `REQ-CLI-TEST-003` When mutating commands exist, tests must cover read-after-
  write verification.
- `REQ-CLI-TEST-004` When an MCP surface exists, CLI and MCP tests must verify
  contract equivalence instead of parallel but different payloads.
>>>>>>> f9fe54d (Finalize phase A planning framework)
