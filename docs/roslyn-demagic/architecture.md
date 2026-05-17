# Roslyn.DeMagic Architecture

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
