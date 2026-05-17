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
