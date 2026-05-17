# roslyn-lint Requirements

## 1. Product Definition

Product requirement IDs:
- `REQ-P-PRODUCT-001` The `roslyn-lint` repository defines one product suite
  composed of:
  - `Roslyn.DeMagic`, a Roslyn analyzer NuGet package
  - `roslyn-lint`, a companion CLI distributed as a .NET tool
- `REQ-P-PRODUCT-002` The product source of truth is this document set, not
  the current implementation. Any existing code that conflicts with these
  requirements may be refactored or deleted during Phase A.

The Phase A goal is to convert the current draft implementation into a
production-ready first release line for `Roslyn.DeMagic` and its companion CLI.

## 1.1 Documentation Structure

Top-level product docs in `docs/` own the suite-level contract:
- [`requirements.md`](./requirements.md)
- [`architecture.md`](./architecture.md)
- [`project-plan.md`](./project-plan.md)

Project-local ownership docs live under:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)
- [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)
- [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)

Phase planning docs live under:
- [`docs/phase-A/plan-phase-A.md`](./phase-A/plan-phase-A.md)

## 2. Scope

Product requirement IDs:
- `REQ-P-SCOPE-001` Phase A must deliver a coherent documentation framework,
  a formal implementation plan, and a production-ready delivery line for
  `Roslyn.DeMagic`.
- `REQ-P-SCOPE-002` If the current code does not satisfy the PRD or these
  requirements, the code must be corrected, replaced, or removed. Existing
  implementation is not grandfathered.
- `REQ-P-SCOPE-003` Phase A must keep the analyzer package and companion CLI
  as separate project responsibilities even when they share repository-level
  planning and validation.

### 2.1 In Scope

- `Roslyn.DeMagic` diagnostic behavior and packaging
- repository-wide analyzer configuration model
- companion CLI behavior needed to run and report lint analysis
- CI, packaging, and release gates for both deliverables
- test coverage that proves rule semantics and packaging behavior

### 2.2 Out of Scope

- code-fix providers for `DM001` or `DM002`
- non-C# language support
- additional analyzers beyond `Roslyn.DeMagic`
- advanced IDE UX beyond standard Roslyn diagnostic behavior

## 3. Configuration Contract

Product requirement IDs:
- `REQ-P-CONFIG-001` Repository-wide analyzer configuration lives under the
  solution root `.roslyn-lint/` directory.
- `REQ-P-CONFIG-002` The configuration model uses two independent files:
  - `.roslyn-lint/config-src.toml`
  - `.roslyn-lint/config-test.toml`
- `REQ-P-CONFIG-003` Source and test configuration are selected once at the
  solution root through `Directory.Build.props`; individual projects must not
  invent competing `AdditionalFiles` wiring.
- `REQ-P-CONFIG-004` Missing or malformed configuration must fail safely:
  analyzers may disable affected rules, but must not crash the host or produce
  undefined behavior.

Detailed implementation obligations are owned by:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)

## 4. Analyzer Product Requirements

Product requirement IDs:
- `REQ-P-DM001-001` `DM001` enforces constant consolidation for public and
  internal constants using the designated file/class contract defined by the
  configuration schema.
- `REQ-P-DM001-002` `DM001` must not report private constants, method-local
  constants, enum member declarations, or other exclusions explicitly defined
  by the `Roslyn.DeMagic` requirements.
- `REQ-P-DM002-001` `DM002` forbids configured string literal values and value
  patterns that indicate domain coupling in general-purpose code.
- `REQ-P-DM002-002` `DM002` matching must support exact, prefix, suffix, and
  substring wildcard forms with configurable case sensitivity.
- `REQ-P-DM002-003` `DM002` applies across the documented string-literal
  contexts and exclusions defined by the project-local requirements.
- `REQ-P-DIAGNOSTICS-001` The analyzer surface must expose stable Roslyn
  diagnostics with documented IDs, categories, default severities, and
  suppressibility.

Detailed analyzer obligations are owned by:
- [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)

## 5. CLI Product Requirements

Product requirement IDs:
- `REQ-P-CLI-001` The `roslyn-lint` CLI must provide a `lint` command that can
  analyze repository code targets and report diagnostics in supported output
  formats.
- `REQ-P-CLI-002` The CLI must remain a thin companion tool. It may orchestrate
  analyzer execution and reporting, but it must not become the source of truth
  for analyzer rule semantics.
- `REQ-P-CLI-003` CLI output must support at least text and JSON forms suitable
  for developer and automation use.
- `REQ-P-CLI-004` CLI exit behavior must distinguish successful analysis from
  blocking diagnostic outcomes according to the documented severity contract.

Detailed CLI obligations are owned by:
- [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)

## 6. Packaging And Release Requirements

Product requirement IDs:
- `REQ-P-PACKAGE-001` `Roslyn.DeMagic` must ship as a Roslyn analyzer package
  with the correct analyzer asset layout for consuming .NET projects.
- `REQ-P-PACKAGE-002` `roslyn-lint` must ship as a .NET tool package.
- `REQ-P-PACKAGE-003` Release packaging must keep analyzer and CLI artifacts
  independently packable and publishable from the same repository release line.
- `REQ-P-RELEASE-001` Tag-driven release automation must restore, build, test,
  pack, and publish both packages before the release is considered complete.

## 7. Validation Requirements

Product requirement IDs:
- `REQ-P-VALIDATION-001` CI must run build and test validation on Linux,
  macOS, and Windows.
- `REQ-P-VALIDATION-002` Analyzer rule behavior must be covered by automated
  tests that prove the documented acceptance criteria for `DM001` and `DM002`.
- `REQ-P-VALIDATION-003` Packaging validation must prove that the analyzer
  package contains the analyzer asset under `analyzers/dotnet/cs` and that the
  CLI package remains installable as a tool.

## 8. Traceability

Project-level ownership references:
- `Roslyn.DeMagic` requirements and architecture:
  - [`docs/roslyn-demagic/requirements.md`](./roslyn-demagic/requirements.md)
  - [`docs/roslyn-demagic/architecture.md`](./roslyn-demagic/architecture.md)
- `roslyn-lint` requirements and architecture:
  - [`docs/roslyn-lint/requirements.md`](./roslyn-lint/requirements.md)
  - [`docs/roslyn-lint/architecture.md`](./roslyn-lint/architecture.md)

Implementation sequencing references:
- [`docs/project-plan.md`](./project-plan.md)
- [`docs/phase-A/plan-phase-A.md`](./phase-A/plan-phase-A.md)
