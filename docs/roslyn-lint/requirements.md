# roslyn-lint Requirements

## 1. Product Definition

`roslyn-lint` is the suite CLI project and the stable top-level executable for
the repository's planned lint-tool family.

It is not a one-off wrapper around `Roslyn.DeMagic`. It is the umbrella
command surface that will front multiple package-owned lint tools over time.

The current CLI implementation is an unapproved spike and may be deleted.

Product requirement IDs:

- `REQ-CLI-PRODUCT-001` `roslyn-lint` remains a first-class project in this
  repository distinct from `Roslyn.DeMagic`.
- `REQ-CLI-PRODUCT-002` The CLI must be designed for AI and system consumption
  first, with human-readable output as a secondary presentation layer.
- `REQ-CLI-PRODUCT-003` No current code or document in this repository is
  grandfathered if it conflicts with this baseline.
- `REQ-CLI-PRODUCT-004` The current `Program.cs` and `Commands/LintCommand.cs`
  spike is disposable and must not constrain the approved implementation line.
- `REQ-CLI-PRODUCT-005` Until the approved contract DTOs and seams exist in
  code, the CLI package must not be treated as the repository's approved
  external release artifact.
- `REQ-CLI-PRODUCT-006` The stable top-level executable name must remain
  `roslyn-lint`.
- `REQ-CLI-PRODUCT-007` The top-level command-family model must mirror the
  `sc-lint` product pattern unless a later ADR changes that decision.
- `REQ-CLI-PRODUCT-008` Package-owned tools must be surfaced through
  `roslyn-lint`; package-local executables or libraries are implementation
  details, not separate public products.
- `REQ-CLI-PRODUCT-009` The first approved lint target is `roslyn-demagic`.
- `REQ-CLI-PRODUCT-010` Future lint-target identifiers must map one-to-one to
  backend package ownership boundaries and be documented before exposure.
- `REQ-CLI-PRODUCT-011` The CLI must scale to a multi-tool suite of roughly
  ten planned lint packages without changing the top-level contract family.

## 1.1 Extensibility Baseline

Extensibility requirement IDs:

- `REQ-CLI-EXT-001` The suite reserves one low-level shared package named
  `Roslyn.Lint.Abstractions` for tool-module integration contracts.
- `REQ-CLI-EXT-002` `Roslyn.Lint.Abstractions` is the planned home for shared
  enums, shared interfaces, tool descriptors, stable tool identifiers, and any
  suite-specific consumer attributes required by future lint packages.
- `REQ-CLI-EXT-003` The suite must prefer standard `.NET` and Roslyn
  suppression/configuration mechanisms such as `#pragma warning`,
  `SuppressMessage`, and `.editorconfig` before introducing custom attributes
  for suppression.
- `REQ-CLI-EXT-004` Custom attributes are allowed only for suite-specific
  semantics not modeled cleanly by standard `.NET` mechanisms, such as boundary
  declarations, tool metadata, or ownership markers.
- `REQ-CLI-EXT-005` `Roslyn.Lint.Core` is not part of the baseline plan and
  must not be introduced until multiple tools prove a real shared-logic need.

## 2. Command Surface Requirements

Command-surface requirement IDs:

- `REQ-CLI-SURFACE-001` The initial top-level command surface must include:
  - `roslyn-lint lint <tool>`
  - `roslyn-lint view <target>`
  - `roslyn-lint check`
  - `roslyn-lint clippy`
  - `roslyn-lint ci`
  - `roslyn-lint version`
- `REQ-CLI-SURFACE-002` The CLI must support named lint profiles:
  - `roslyn-lint lint fast`
  - `roslyn-lint lint full`
  - `roslyn-lint lint ci`
- `REQ-CLI-SURFACE-003` Top-level `roslyn-lint ci` must remain distinct from
  `roslyn-lint lint ci`; the first is lint-plus-tests orchestration and the
  second is lint-only profile execution.
- `REQ-CLI-SURFACE-004` The `view` command family is reserved as a stable
  top-level grouping, but its target inventory may remain narrower than
  `lint`.
- `REQ-CLI-SURFACE-005` The primary analyzer entry path must be
  `roslyn-lint lint <tool>`, with `roslyn-demagic` as the first documented
  tool identifier.
- `REQ-CLI-SURFACE-006` Future backend implementation changes must not require
  changing the public top-level command families or dotted command identifiers.
- `REQ-CLI-SURFACE-007` Rust-specific capability targets from `sc-lint`, such
  as `xwin`, are not part of the Roslyn baseline unless a later ADR introduces
  an equivalent `.NET` capability target explicitly.

## 3. Core Contract

Contract requirement IDs:

- `REQ-CLI-CONTRACT-001` Every non-interactive command must support `--json`.
- `REQ-CLI-CONTRACT-002` `--json` mode is the normative output contract.
- `REQ-CLI-CONTRACT-003` Success and failure results must use one stable JSON
  envelope family across command families.
- `REQ-CLI-CONTRACT-004` Important machine data must not be available only in
  prose or colorized text output.
- `REQ-CLI-CONTRACT-005` The CLI must expose explicit request and response
  models for structured operations rather than building JSON from formatted
  strings.
- `REQ-CLI-CONTRACT-006` The baseline JSON envelope must include:
  `ok`, `command`, and exactly one of `data` or `error`. Optional
  `diagnostics` may be included on either path.
- `REQ-CLI-CONTRACT-007` Exit behavior must be deterministic: success exits
  zero, contract or operational failure exits non-zero, and `--json` mode must
  still emit the contract envelope on failure.
- `REQ-CLI-CONTRACT-008` Commands that accept structured input must support a
  reusable machine input path such as stdin JSON, `--input`, or `--file`.
- `REQ-CLI-CONTRACT-009` The `command` field must use a stable dotted
  identifier derived from the selected top-level path.
- `REQ-CLI-CONTRACT-010` Delegated backend payloads must be normalized into the
  top-level envelope rather than leaking backend-native top-level shapes.

## 4. Error Contract

Error requirement IDs:

- `REQ-CLI-ERROR-001` Errors in `--json` mode must be structured JSON, not
  prose-only stderr output.
- `REQ-CLI-ERROR-002` Error results must expose a stable machine code and a
  broader error kind or category.
- `REQ-CLI-ERROR-003` Error results must include actionable guidance when a
  caller can recover.
- `REQ-CLI-ERROR-004` The top-level CLI error categories must distinguish:
  `usage`, `config`, `capability`, `backend_failure`,
  `backend_protocol`, and `internal`.
- `REQ-CLI-ERROR-005` The baseline error object must include:
  `kind`, `code`, `message`, and may include `details` and
  `suggested_action`.
- `REQ-CLI-ERROR-006` Backend-specific domain errors may preserve deeper codes
  or diagnostics beneath the CLI surface, but the top-level failure family must
  still use `CliError`.

## 5. MCP-Ready Contract

Compatibility requirement IDs:

- `REQ-CLI-MCP-001` CLI request and response DTOs must be reusable by a future
  MCP wrapper without business-payload reshaping.
- `REQ-CLI-MCP-002` Human-readable formatting must be isolated from business
  payload construction.
- `REQ-CLI-MCP-003` Future MCP tests must be able to reuse CLI JSON fixtures.

## 6. Orchestration And Dispatch Requirements

Dispatch requirement IDs:

- `REQ-CLI-DISPATCH-001` The CLI must load repo configuration before backend
  dispatch.
- `REQ-CLI-DISPATCH-002` The CLI must normalize output and exit behavior across
  backend tool packages.
- `REQ-CLI-DISPATCH-003` The CLI may support both in-process `.NET` library
  backends and delegated process backends during migration periods.
- `REQ-CLI-DISPATCH-004` Backend-specific flags or wire formats must remain
  behind the top-level CLI contract boundary.
- `REQ-CLI-DISPATCH-005` Backend tool packages must not depend on each other
  directly just to satisfy top-level command orchestration.

## 7. Auditability

Auditability requirement IDs:

- `REQ-CLI-AUDIT-001` Every mutating command must have a corresponding read,
  list, show, or status command that can verify resulting state.
- `REQ-CLI-AUDIT-002` Mutating JSON responses must report enough state for
  deterministic automation, including target identity and resulting status.
- `REQ-CLI-AUDIT-003` Exit codes alone are insufficient proof of success for a
  mutating command.

## 8. External Integration Simulation

Simulation requirement IDs:

- `REQ-CLI-SIM-001` Any future external integration must sit behind a swappable
  adapter boundary.
- `REQ-CLI-SIM-002` External-integration tests must run against a stateful
  simulator rather than live-only infrastructure.
- `REQ-CLI-SIM-003` Simulator-backed tests must support read-after-write and
  negative-path behavior.

## 9. .NET Implementation Requirements

Implementation requirement IDs:

- `REQ-CLI-DOTNET-001` The approved parser and command-registration baseline is
  `System.CommandLine`.
- `REQ-CLI-DOTNET-002` The CLI must use explicit DTO types for request and
  response payloads when structured contracts are involved.
- `REQ-CLI-DOTNET-003` JSON serialization settings must be shared across CLI
  and future MCP entrypoints.
- `REQ-CLI-DOTNET-004` The command layer must stay thin over a reusable
  dispatch and operation layer.
- `REQ-CLI-DOTNET-005` If the current Spectre-based command structure blocks
  compliance with this baseline, it must be replaced instead of preserved.
- `REQ-CLI-DOTNET-006` The current Spectre-based spike may remain only as a
  temporary local tool surface; it is not the approved architectural endpoint.
- `REQ-CLI-DOTNET-007` The implementation should use source-generated
  `System.Text.Json` context types for stable CLI and future MCP serialization.
- `REQ-CLI-DOTNET-008` Command handlers must not construct anonymous
  `Dictionary<string, object>` machine payloads when explicit DTOs are
  practical.
- `REQ-CLI-DOTNET-009` Low-level extensibility contracts must remain free of
  parser-library and command-host dependencies so package-owned tools can reuse
  them without taking a dependency on the CLI entrypoint implementation.

## 10. Validation Requirements

Validation requirement IDs:

- `REQ-CLI-TEST-001` Tests must assert `--json` machine contracts directly.
- `REQ-CLI-TEST-002` Tests must cover success and failure envelopes.
- `REQ-CLI-TEST-003` Tests must cover command-family and profile parsing.
- `REQ-CLI-TEST-004` Tests must cover top-level normalization for delegated
  backend success and failure paths.
- `REQ-CLI-TEST-005` When mutating commands exist, tests must cover read-after-
  write verification.
- `REQ-CLI-TEST-006` When an MCP surface exists, CLI and MCP tests must verify
  contract equivalence instead of parallel but different payloads.
