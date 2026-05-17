# roslyn-lint CLI Requirements

## 1. Purpose

This document defines the `roslyn-lint` CLI project requirements.

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

## 4. Command Requirements

- `REQ-CLI-CMD-001` The CLI must expose a `lint` command.
- `REQ-CLI-CMD-002` The `lint` command must accept a path target that can point
  to a file or directory.
- `REQ-CLI-CMD-003` The CLI must support `text` and `json` output formats.
- `REQ-CLI-CMD-004` The CLI must support include and exclude filtering for
  target file selection.
- `REQ-CLI-CMD-005` The CLI must support disabling colored output.

## 5. Analysis Requirements

- `REQ-CLI-ANALYSIS-001` The CLI must orchestrate analyzer execution without
  redefining `Roslyn.DeMagic` rule semantics.
- `REQ-CLI-ANALYSIS-002` The CLI must either analyze code through a
  project-aware path that honors the documented config model or clearly narrow
  its supported execution mode in the implementation and docs.
- `REQ-CLI-ANALYSIS-003` If the current direct-file compilation model cannot
  faithfully represent the documented analyzer behavior, it must be refactored
  or removed rather than preserved as a misleading product path.

## 6. Output And Exit Requirements

- `REQ-CLI-OUTPUT-001` Text output must include file location, severity,
  diagnostic ID, and message.
- `REQ-CLI-OUTPUT-002` JSON output must provide machine-readable issue records.
- `REQ-CLI-EXIT-001` CLI exit behavior must distinguish success from blocking
  diagnostics according to the documented severity policy.

## 7. Packaging Requirements

- `REQ-CLI-PACKAGE-001` The CLI must remain packable as a .NET tool with
  command name `roslyn-lint`.
- `REQ-CLI-PACKAGE-002` CLI packaging must remain separate from analyzer
  packaging even when both ship from the same repository release.

## 8. Validation Requirements

- `REQ-CLI-TEST-001` Automated tests must verify settings validation and
  supported output modes.
- `REQ-CLI-TEST-002` Phase A must add or update tests for any changed CLI
  behavior introduced to align the tool with the documented analyzer contract.
