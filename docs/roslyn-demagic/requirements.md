# Roslyn.DeMagic Requirements

## 1. Purpose

This document defines the `Roslyn.DeMagic` project requirements.

Suite-level product behavior remains defined in
[`../requirements.md`](../requirements.md).

## 2. Ownership

`Roslyn.DeMagic` owns:
- analyzer rule semantics for `DM001` and `DM002`
- configuration ingress from `AdditionalFiles`
- diagnostic descriptor metadata
- analyzer packaging and Roslyn host compatibility
- analyzer-specific tests

`Roslyn.DeMagic` does not own:
- CLI command parsing
- terminal or JSON output formatting
- tool-install packaging for `roslyn-lint`
- repository phase planning

## 3. Requirement Namespace

The `Roslyn.DeMagic` project uses the `REQ-DEMAGIC-*` namespace.

## 4. Configuration Requirements

- `REQ-DEMAGIC-CONFIG-001` The analyzer must consume exactly one applicable
  config file per project via `AdditionalFiles`, selected by repository-level
  wiring.
- `REQ-DEMAGIC-CONFIG-002` The configuration schema must support independent
  `[dm001]` and `[dm002]` sections as defined by the PRD.
- `REQ-DEMAGIC-CONFIG-003` Missing config must disable analyzer rules
  gracefully rather than crash the host.
- `REQ-DEMAGIC-CONFIG-004` Malformed config must produce a clear diagnostic or
  equivalent failure signal and disable only the affected rule behavior.
- `REQ-DEMAGIC-CONFIG-005` Source and test configuration files are fully
  independent. No merge or inheritance is allowed between them.

## 5. DM001 Requirements

- `REQ-DEMAGIC-DM001-001` `DM001` reports public and internal `const` fields
  declared outside the configured designated file.
- `REQ-DEMAGIC-DM001-002` When `designated_class` is configured, `DM001` also
  requires the constant to live in the configured containing type.
- `REQ-DEMAGIC-DM001-003` `DM001` does not report:
  - private constants
  - protected constants
  - method-local constants
  - `static readonly` fields in v1
- `REQ-DEMAGIC-DM001-004` `DM001` uses diagnostic ID `DM001`, category
  `roslyn-lint.Organization`, and default severity `Warning`.
- `REQ-DEMAGIC-DM001-005` `DM001` severity must be overridable from config and
  remain suppressible through normal Roslyn mechanisms.

## 6. DM002 Requirements

- `REQ-DEMAGIC-DM002-001` `DM002` reports string literals whose values match a
  configured forbidden pattern.
- `REQ-DEMAGIC-DM002-002` Pattern matching must support:
  - exact match
  - prefix wildcard
  - suffix wildcard
  - substring wildcard
- `REQ-DEMAGIC-DM002-003` Matching is case-insensitive by default and becomes
  case-sensitive only when explicitly configured.
- `REQ-DEMAGIC-DM002-004` `DM002` applies to string literal expressions across
  documented contexts such as comparisons, assignments, returns, switch cases,
  and method arguments.
- `REQ-DEMAGIC-DM002-005` `DM002` does not analyze:
  - comments
  - XML documentation
  - interpolated string holes in v1
- `REQ-DEMAGIC-DM002-006` `DM002` uses diagnostic ID `DM002`, category
  `roslyn-lint.DomainBoundary`, and default severity `Error`.
- `REQ-DEMAGIC-DM002-007` `DM002` severity must be overridable from config and
  remain suppressible through normal Roslyn mechanisms.

## 7. Packaging Requirements

- `REQ-DEMAGIC-PACKAGE-001` The analyzer package targets `netstandard2.0` for
  Roslyn host compatibility.
- `REQ-DEMAGIC-PACKAGE-002` The published analyzer DLL must be packed under
  `analyzers/dotnet/cs`.
- `REQ-DEMAGIC-PACKAGE-003` Analyzer package dependencies must remain
  development-time only and must not introduce runtime dependencies into
  consuming projects.
- `REQ-DEMAGIC-PACKAGE-004` Analyzer release tracking files must be updated
  whenever rule surface or shipped diagnostics change.

## 8. Validation Requirements

- `REQ-DEMAGIC-TEST-001` Automated tests must prove the Phase A acceptance
  criteria for `DM001`.
- `REQ-DEMAGIC-TEST-002` Automated tests must prove the Phase A acceptance
  criteria for `DM002`.
- `REQ-DEMAGIC-TEST-003` Automated tests must cover missing-config and
  malformed-config behavior.
- `REQ-DEMAGIC-TEST-004` Packaging validation must prove the analyzer asset is
  emitted in the correct NuGet location.
