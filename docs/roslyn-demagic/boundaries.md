# Roslyn.DeMagic Boundary Inventory

This document captures the analyzer-owned boundaries for Phase A.

The current spike implementation does not define these boundaries cleanly and
may be removed when implementation begins.

## ConfigurationFileSelection

Purpose:

- owns resolution of the applicable `.roslyn-lint` configuration file delivered
  through `AdditionalFiles`

Notes:

- this boundary decides which config document is relevant to the current
  project
- it must consume only the `AdditionalFiles` inputs provided by the Roslyn host
- it does not own TOML parsing policy beyond locating the source input
- the preferred implementation seam is `IAdditionalFileConfigSelector`
- the preferred selected payload type is `AdditionalFileConfigSelection`

## ConfigurationModelParsing

Purpose:

- owns parsing analyzer TOML into explicit internal configuration models

Notes:

- this boundary produces typed models for `DM001` and `DM002`
- syntax callbacks must not parse raw TOML strings directly
- the preferred implementation is a shared immutable config object created once
  per compilation
- the preferred loader seam is `IDeMagicConfigLoader`
- the preferred parsing seam is `ITomlConfigParser`
- the preferred payload types are `DeMagicConfig`, `Dm001Options`, and
  `Dm002Options`

## DiagnosticSeverityMapping

Purpose:

- owns mapping config severity values into Roslyn diagnostic behavior

Notes:

- this boundary keeps severity policy centralized
- analyzer rule callbacks should not duplicate severity parsing
- rule callbacks should consume precomputed descriptor or severity state rather
  than rebuild it per syntax node
- the preferred closed vocabulary type is the `ConfiguredSeverity` enum

## DM001ConstantConsolidationRule

Purpose:

- owns detection of public and internal constant declarations that violate the
  designated file and optional designated class rules

Notes:

- this boundary owns declaration-based analysis only
- it does not own generic numeric literal detection
- replacement with a new analyzer type name is preferred over keeping the
  `MagicNumberAnalyzer` type

## DM002ForbiddenLiteralRule

Purpose:

- owns detection of string literals that match forbidden exact, prefix, suffix,
  or substring patterns

Notes:

- this boundary owns literal matching against compiled pattern definitions
- it does not own comment or documentation scanning in v1
- the approved analyzer type is `DM002ForbiddenStringLiteralAnalyzer`
- the preferred payload types are `ForbiddenPattern`,
  `CompiledForbiddenPattern`, and the `ForbiddenPatternKind` enum

## ForbiddenPatternCompilation

Purpose:

- owns compilation of config pattern strings into efficient internal matchers

Notes:

- this boundary should remain testable without Roslyn callback wiring
- matching semantics must stay aligned to the PRD
- the preferred compilation seam is `IForbiddenPatternCompiler`

## DiagnosticDescriptorPolicy

Purpose:

- owns stable IDs, categories, and message templates for analyzer diagnostics

Notes:

- the package must not preserve the current spike's "magic number" and "magic
  string" framing where that conflicts with the PRD

## AnalyzerReleaseMetadata

Purpose:

- owns shipped and unshipped analyzer release records and their alignment with
  the real diagnostic set

Notes:

- release metadata must track the approved rules, not the spike behavior
