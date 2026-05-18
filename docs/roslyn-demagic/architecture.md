# Roslyn.DeMagic Architecture

## 1. Overview

`Roslyn.DeMagic` is a standalone analyzer package that reads one project-scoped
configuration file and emits Roslyn diagnostics for two rule families:

- `DM001` constant consolidation
- `DM002` forbidden string literals

The current `MagicNumberAnalyzer` spike does not match this target
architecture and may be deleted. The old `MagicStringAnalyzer` spike has been
replaced by `DM002ForbiddenStringLiteralAnalyzer`.

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

- `Configuration/IAdditionalFileConfigSelector.cs` for selecting the applicable
  analyzer config payload from `AdditionalFiles`
- `Configuration/ITomlConfigParser.cs` for TOML-to-model conversion behind a
  replaceable parsing seam
- `Configuration/DeMagicConfig.cs` for the full analyzer config model
- `Configuration/Dm001Options.cs` for `DM001` rule settings
- `Configuration/Dm002Options.cs` for `DM002` rule settings
- `Configuration/ConfiguredSeverity.cs` for the shared severity enum
- `Configuration/AdditionalFileConfigSelection.cs` for the selected config
  payload record/struct
- `Configuration/DeMagicConfigLoader.cs` for `AdditionalFiles` selection and
  TOML parsing
- `Diagnostics/DeMagicDiagnosticDescriptors.cs` for centralized descriptor
  ownership
- `Patterns/ForbiddenPattern.cs` for the configured pattern record/struct
- `Patterns/ForbiddenPatternKind.cs` for exact/prefix/suffix/substring
  matching semantics
- `Patterns/CompiledForbiddenPattern.cs` for the compiled matcher input
- `Patterns/IForbiddenPatternCompiler.cs` for pattern compilation ownership
- `Patterns/ForbiddenPatternMatcher.cs` for compiled `DM002` match logic
- `Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`

The current `MagicNumberAnalyzer` file should be deleted if keeping it would
preserve rejected semantics or misleading type names.

## 3.2 Planned Interfaces, Records/Structs, and Enums

The Phase A analyzer design expects these named types to exist in the
implementation plan:

- `IAdditionalFileConfigSelector`
- `ITomlConfigParser`
- `IForbiddenPatternCompiler`
- `DeMagicConfig`
- `Dm001Options`
- `Dm002Options`
- `AdditionalFileConfigSelection`
- `ForbiddenPattern`
- `CompiledForbiddenPattern`
- `ConfiguredSeverity`
- `ForbiddenPatternKind`

Type guidance:

- use interfaces for replaceable boundary seams
- use immutable records or readonly structs for config and pattern payloads
- use enums only for stable closed vocabularies such as severity and pattern
  kind

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
- packaged-consumer behavior through a project that references the locally built
  `Roslyn.DeMagic` NuGet package from a local feed
- requirement-to-sample traceability for every approved rule and documented
  corner case

The test suite must be built around PRD behavior, not around preserving the
current spike's literal-detection semantics.

If the current spike structure blocks compliance with this architecture,
deletion and clean replacement is preferred over preserving the spike.
