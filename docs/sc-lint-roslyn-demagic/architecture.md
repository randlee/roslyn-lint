# sc.lint.roslyn.demagic Architecture

## 1. Overview

`sc.lint.roslyn.demagic` is a standalone analyzer package that reads one project-scoped
configuration file and emits Roslyn diagnostics for two rule families:

- `DM001` constant consolidation
- `DM002` forbidden string literals

The current `MagicNumberAnalyzer` spike does not match this target
architecture and may be deleted. The old `MagicStringAnalyzer` spike has been
replaced by `DM002ForbiddenStringLiteralAnalyzer`.

Relevant ADRs:

- `ADR-001`
- `ADR-002`
- `ADR-006`

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

## 3.1 Delivered Phase A Implementation Units

The delivered Phase A implementation shape is:

- `configuration/IAdditionalFileConfigSelector.cs` for selecting the applicable
  analyzer config payload from `AdditionalFiles`
- `configuration/ITomlConfigParser.cs` for TOML-to-model conversion behind a
  replaceable parsing seam
- `configuration/DeMagicConfig.cs` for the full analyzer config model
- `configuration/Dm001Options.cs` for `DM001` rule settings
- `configuration/Dm002Options.cs` for `DM002` rule settings
- `configuration/ConfiguredSeverity.cs` for the shared severity enum
- `configuration/AdditionalFileConfigSelection.cs` for the selected config
  payload record/struct
- `configuration/DeMagicConfigLoader.cs` for `AdditionalFiles` selection and
  TOML parsing
- `diagnostics/DeMagicDiagnosticDescriptors.cs` for centralized descriptor
  ownership
- `patterns/ForbiddenPattern.cs` for the configured pattern record/struct
- `patterns/ForbiddenPatternKind.cs` for exact/prefix/suffix/substring
  matching semantics
- `patterns/CompiledForbiddenPattern.cs` for the compiled matcher input
- `patterns/IForbiddenPatternCompiler.cs` for pattern compilation ownership
- `patterns/ForbiddenPatternMatcher.cs` for compiled `DM002` match logic
- `analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`

The current `MagicNumberAnalyzer` file should be deleted if keeping it would
preserve rejected semantics or misleading type names.

## 3.2 Delivered Interfaces, Records/Structs, and Enums

The delivered Phase A analyzer boundary uses these named types:

- `IAdditionalFileConfigSelector`
- `IDeMagicConfigLoader`
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
- `ForbiddenPatternMatcher`

Type guidance:

- use interfaces for replaceable boundary seams
- use immutable records or readonly structs for config and pattern payloads
- use enums only for stable closed vocabularies such as severity and pattern
  kind

## 3. Configuration Flow

The solution root owns `Directory.Build.props` wiring for:

- `.sc-lint-roslyn/config-src.toml`
- `.sc-lint-roslyn/config-test.toml`

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

Boundary ownership detail is defined in `docs/sc-lint-roslyn-demagic/boundaries.md`.

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
  `sc.lint.roslyn.demagic` NuGet package from a local feed
- requirement-to-sample traceability for every approved rule and documented
  corner case
- a structured expected-diagnostics manifest that scripts can compare against
  actual package-consumer build output
- CI execution of the packaged-consumer validation path before GitHub Packages
  publication is considered passing

The package-validation support types used by that path are delivered compiled
types owned by `tests/sc.lint.roslyn.demagic.tests/packagevalidation/`:

- `ExpectedPackageDiagnostic`
- `PackageValidationManifest`
- `PackageValidationResult`
- `PackageValidationSampleKind`
- `ProductionReadinessChecklistRow`

The analyzer sample-corpus and traceability support types are delivered
compiled test-support types owned by `tests/sc.lint.roslyn.demagic.tests/testing/`:

- `ExpectedDiagnostic`
- `RequirementTraceabilityRow`
- `AnalyzerSampleKind`

The test suite must be built around PRD behavior, not around preserving the
current spike's literal-detection semantics.

If the current spike structure blocks compliance with this architecture,
deletion and clean replacement is preferred over preserving the spike.

## 7. Phase A Delivery State

The shippable Phase A analyzer set is:

- `DM001` constant consolidation
- `DM002` forbidden string literals

The production-testing contract for that set is:

- release metadata and `AnalyzerReleases.*` describe only those two rules
- `tests/sc.lint.roslyn.demagic.tests/TestMatrix.md` provides requirement traceability
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` closes supported analyzer
  permutations explicitly
- `examples/sc.lint.roslyn.demagic.package-smoke/` validates the locally packed NuGet
  package through a normal consumer project
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json` is the authoritative
  machine-readable packaged-consumer expectation set for Phase A v0.2.0

## 8. Phase B Local Dogfooding Architecture

Phase B turns the shipped analyzer inward on this repository's own projects.

Architectural rules:

- local dogfooding should use an analyzer reference wired from repository build
  configuration rather than requiring a pack-and-install loop for every inner
  iteration
- the first dogfooding pass must scope source projects and test projects
  separately so `.sc-lint-roslyn/config-src.toml` and
  `.sc-lint-roslyn/config-test.toml` remain distinct adoption inputs
- initial dogfooding should target the repository-owned C# source and test
  projects; the package-smoke example remains a separate packaged-consumer
  validation surface
- findings generated by local dogfooding must flow into an explicit inventory
  and remediation policy document rather than being treated as ad hoc console
  output
- unexpected or non-predictable analyzer behavior must be turned into explicit
  follow-up issues, not buried inside the findings inventory
- B3 package-surface cleanup must publish the analyzer consumer boundary in
  `docs/sc-lint-roslyn-demagic/package-usage.md`, including package id,
  package-reference shape, supported target frameworks, and repository URL
