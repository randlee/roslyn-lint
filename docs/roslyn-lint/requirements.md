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
