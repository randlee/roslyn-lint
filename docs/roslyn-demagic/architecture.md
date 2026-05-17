# Roslyn.DeMagic Architecture

<<<<<<< HEAD
## 1. Purpose

This document defines the `Roslyn.DeMagic` project architecture.

It complements the suite-level architecture in
[`../architecture.md`](../architecture.md) and owns project-local structure.

## 2. Architectural Rules

- analyzer rule semantics live in `Roslyn.DeMagic`, not in the CLI
- configuration parsing must have one shared implementation path for both
  `DM001` and `DM002`
- diagnostic metadata must be defined once per rule and reused consistently
- packaging behavior must preserve Roslyn analyzer host compatibility
- current spike implementations may be replaced if they do not satisfy these
  rules

## 3. Target Component Model

The project should converge on these component responsibilities:

### 3.1 Diagnostic Contract Layer

Owns:
- stable diagnostic IDs
- titles and messages
- categories
- default severities
- rule descriptions

### 3.2 Configuration Layer

Owns:
- loading the selected `AdditionalFiles` config
- parsing TOML into a typed configuration model
- config validation
- safe fallback when config is missing or malformed

### 3.3 Rule Layer

Owns:
- `DM001` constant consolidation analysis
- `DM002` forbidden string literal analysis
- syntax and semantic exclusion handling
- reporting diagnostics through the shared contract layer

### 3.4 Packaging Layer

Owns:
- analyzer project shape
- `netstandard2.0` host compatibility
- NuGet analyzer asset placement
- analyzer release-tracking file integration

## 4. Phase A Architectural Direction

The current code already contains `MagicNumberAnalyzer` and
`MagicStringAnalyzer`, but those are only a provisional baseline.

Phase A architectural direction:
- replace simple magic-number/magic-string behavior with the documented
  `DM001` and `DM002` contracts
- centralize config loading so rule behavior is not hardcoded ad hoc
- align diagnostic categories and default severities with the formal
  requirements
- keep packaging and analyzer-release files in sync with the documented rule
  surface

## 5. Implementation Boundary

`Roslyn.DeMagic` must remain an analyzer package, not a general-purpose CLI or
application runtime. Any helper code added during Phase A must support analyzer
execution, config loading, packaging, or testability for the analyzer surface.
=======
## 1. Overview

`Roslyn.DeMagic` is a standalone analyzer package that reads one project-scoped
configuration file and emits Roslyn diagnostics for two rule families:

- `DM001` constant consolidation
- `DM002` forbidden string literals

The current `MagicNumberAnalyzer` and `MagicStringAnalyzer` spike does not
match this target architecture and may be deleted.

Relevant ADRs:

- `ADR-001`
- `ADR-002`

## 2. Inputs and Outputs

Inputs:

- C# syntax and semantic context from the Roslyn analyzer host
- one `AdditionalFiles` TOML configuration file for the current project

Outputs:

- Roslyn diagnostics `DM001` and `DM002`

Architectural rules:

- configuration loading must happen inside the analyzer package boundary
- analyzer execution must remain deterministic for the current syntax tree and
  config input
- missing or invalid config must degrade predictably and not crash analysis

## 3.1 Planned Implementation Units

The preferred replacement-oriented implementation shape is:

- `Configuration/DeMagicConfig.cs` for the full analyzer config model
- `Configuration/DeMagicConfigLoader.cs` for `AdditionalFiles` selection and
  TOML parsing
- `Diagnostics/DeMagicDiagnosticDescriptors.cs` for centralized descriptor
  ownership
- `Patterns/ForbiddenPatternMatcher.cs` for compiled `DM002` match logic
- `Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`

The current `MagicNumberAnalyzer` and `MagicStringAnalyzer` files should be
deleted if keeping them would preserve rejected semantics or misleading type
names.

## 3. Configuration Flow

The solution root owns `Directory.Build.props` wiring for:

- `.roslyn-lint/config-src.toml`
- `.roslyn-lint/config-test.toml`

The analyzer architecture must:

- determine which config file was passed for the current project
- parse the TOML schema once per compilation start and reuse an immutable
  configuration model for all syntax callbacks in that compilation
- expose rule configuration to `DM001` and `DM002` through explicit internal
  models rather than ad hoc string parsing inside syntax actions

## 4. Rule Architecture

### 4.1 DM001

`DM001` should analyze type-member constant declarations against an explicit
configuration model:

- designated file name
- optional designated class name
- enablement
- configured severity

The rule should bind to declaration forms that the PRD actually governs, rather
than reporting generic numeric literal usage.

### 4.2 DM002

`DM002` should analyze string literal syntax against compiled forbidden-pattern
matchers.

The rule architecture should separate:

- config parsing
- pattern compilation
- per-literal matching
- diagnostic creation

This keeps matching logic testable without entangling it with Roslyn callback
registration.

Boundary ownership detail is defined in `docs/roslyn-demagic/boundaries.md`.

## 5. Severity and Diagnostic Construction

The analyzer must map configured severities into Roslyn diagnostic descriptors
or effective reporting behavior without inventing a second severity system.

Architectural rules:

- diagnostic IDs stay stable
- diagnostic messages remain rule-specific
- suppression remains standard Roslyn behavior

## 6. Testing Architecture

Tests should validate:

- rule matching behavior
- rule non-matching behavior
- config parsing and missing-config behavior
- invalid-config fail-closed behavior
- severity mapping
- shipped and unshipped analyzer release metadata alignment

The test suite must be built around PRD behavior, not around preserving the
current spike's literal-detection semantics.

If the current spike structure blocks compliance with this architecture,
deletion and clean replacement is preferred over preserving the spike.
>>>>>>> f9fe54d (Finalize phase A planning framework)
