# Roslyn.DeMagic Architecture

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
