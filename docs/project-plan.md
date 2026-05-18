# Roslyn Lint Suite Project Plan

## 1. Goal

Establish a formal documentation and delivery framework for the repository
while driving the first approved implementation line for `Roslyn.DeMagic`.

The repository does not assume the current code is correct. If the existing
implementation conflicts with approved requirements or architecture, it may be
deleted and replaced.

## 2. Deliverables

Phase A deliverables:

- suite-level requirements, architecture, and project plan
- project-level requirements and architecture for `Roslyn.DeMagic`
- project-level requirements and architecture for `roslyn-lint`
- project-level boundary inventories for both projects
- accepted repository ADRs for enforceable Phase A decisions
- a sprinted Phase A plan
- a PRD-aligned `Roslyn.DeMagic` v1 implementation
- a CLI design baseline aligned with the repository's AI-first CLI rules
- a production-ready analyzer validation path that covers every approved rule
- a packaged-consumer example proving the built analyzer works through a local
  feed
- CI gates that validate the packaged-consumer path before merge
- GitHub Packages publication for repo-produced packages, with the first
  NuGet.org release remaining manual and documented

## 3. Project Inventory

Current project inventory:

- `src/Roslyn.DeMagic`
- `src/Roslyn.Lint`
- `tests/Roslyn.DeMagic.Tests`
- `tests/Roslyn.Lint.Tests`

Owned project documents:

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/documentation-guidelines.md`
- `docs/adr/INDEX.md`

## 4. Work Sequence

### Phase A: Formal Baseline and DeMagic v1

Phase A is the active planning line for this repository.

Execution branch:

- `integration/phase-A`

Merge target:

- `develop`

| Sprint | Scope | Required outcome |
| --- | --- | --- |
| A0 | Documentation reset | Replace placeholders and unapproved assumptions with formal suite and project docs |
| A1 | Analyzer foundation | Add reusable configuration and forbidden-pattern infrastructure for `Roslyn.DeMagic` |
| A2 | `DM002` forbidden-pattern analyzer | Align forbidden-string analysis, config parsing, and analyzer validation with the PRD |
| A3 | `DM002` hardening and release alignment | Remove remaining spike leftovers, align release metadata, and route analyzer seams through interfaces |
| A4 | Packaging and CLI baseline correction | Finalize analyzer packaging gates and lock the CLI design baseline to AI-first contract rules |
| A5 | CLI foundation and abstractions package | Replace the Spectre spike with the first working `System.CommandLine` host and shared tool-module abstractions |
| A6 | DeMagic backend integration and first usable lint flow | Deliver `roslyn-lint lint demagic` and the first usable `lint fast` smoke path |
| A7 | Profiles plus check, clippy, and ci workflows | Deliver reusable lint profiles and the first .NET-native `check`, `clippy`, and `ci` workflows |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Harden the multi-tool CLI surface and delegated backend seams |
| A9 | `DM001` completion and rule parity | Implement the missing constant-consolidation analyzer behavior and close the rule gap |
| A10 | Analyzer sample corpus and rule matrix | Add exhaustive analyzer samples and traceability for every rule and corner case |
| A11 | Packaged consumer validation | Pack the analyzer and consume it from a normal project via a local feed |
| A12 | Production-readiness convergence | Align package metadata, docs, sample validation, and readiness evidence to the shippable analyzer set |
| A13 | CI publish and manual release handoff | Add CI package-consumer gates, GitHub Packages publication, and the documented manual NuGet.org first release path |

Phase A must not treat the current CLI spike as an approved product contract.

Phase A continuation after the initial CLI-baseline and analyzer-foundation
sprints is:

- `A9` closes the missing `DM001` implementation so the analyzer actually
  matches the approved rule inventory.
- `A10` adds the exhaustive analyzer sample corpus and requirement traceability
  matrix.
- `A11` validates the built analyzer as a locally packed package consumed from
  a normal project.
- `A12` converges docs, manifests, release metadata, and readiness evidence
  onto one real shippable analyzer set.
- `A13` adds CI enforcement for packaged-consumer validation and staged
  publication, while keeping the first NuGet.org release manual.

### Phase A Implementation Inventory

Phase A implementation work is expected to touch, add, delete, or replace
these code paths:

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs` deleted in A3
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `examples/Roslyn.DeMagic.PackageSmoke/`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`
- `eng/roslyn-demagic-package-expected-diagnostics.json`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/`
- `docs/phase-A/production-readiness-checklist.md`
- `docs/releasing.md`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/`
- `tests/Roslyn.DeMagic.Tests/TestData/README.md`
- `tests/Roslyn.DeMagic.Tests/TestMatrix.md`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

Planned replacement-oriented analyzer implementation units:

- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/Dm001Options.cs`
- `src/Roslyn.DeMagic/Configuration/Dm002Options.cs`
- `src/Roslyn.DeMagic/Configuration/ConfiguredSeverity.cs`
- `src/Roslyn.DeMagic/Configuration/AdditionalFileConfigSelection.cs`
- `src/Roslyn.DeMagic/Configuration/IAdditionalFileConfigSelector.cs`
- `src/Roslyn.DeMagic/Configuration/ITomlConfigParser.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternKind.cs`
- `src/Roslyn.DeMagic/Patterns/CompiledForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/IForbiddenPatternCompiler.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`

Planned replacement-oriented CLI implementation units when CLI work resumes:

- `src/Roslyn.Lint.Abstractions/Roslyn.Lint.Abstractions.csproj`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Commands/RegisterVersionCommand.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliError.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliDiagnostic.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintFinding.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ViewResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CheckRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CheckResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/VersionResult.cs`
- `src/Roslyn.Lint/Dispatch/IBackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/IBackendProcessRunner.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Operations/ILintToolOperation.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/ICheckOperation.cs`
- `src/Roslyn.Lint/Operations/IClippyOperation.cs`
- `src/Roslyn.Lint/Operations/ICiOperation.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `src/Roslyn.Lint/Adapters/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Operations/`

Planned package-validation support units:

- `tests/Roslyn.DeMagic.Tests/PackageValidation/ExpectedPackageDiagnostic.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationManifest.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationResult.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationSampleKind.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/ProductionReadinessChecklistRow.cs`

## 5. Execution Rules

- requirements and architecture documents are authoritative over the current
  spike implementation
- analyzer-first work takes precedence over speculative CLI feature work until
  the analyzer baseline is approved
- after A8, Phase A work returns to analyzer production-readiness and package
  consumer validation before any further CLI scope is considered
- GitHub Packages publication work remains analyzer-first support work and must
  not displace unfinished analyzer rule, sample, or package-consumer gaps
- future CLI implementation must inherit the contract rules defined in
  `docs/roslyn-lint/requirements.md` and `docs/roslyn-lint/architecture.md`
- if spike code does not comply with approved requirements or architecture, the
  preferred action is full removal and replacement rather than incremental
  editing to preserve unapproved structure
- implementation planning must use the project boundary documents to keep
  analyzer logic, CLI contract logic, and future adapter logic from collapsing
  into one project
- if a current file name or class name encodes the rejected spike semantics,
  replacement with new files and types is preferred over keeping the old names
  and editing their internals

## 6. Phase A Acceptance

Phase A planning is complete only when:

- the repo documentation framework exists and is internally consistent
- `Roslyn.DeMagic` behavior is planned against the PRD rather than against the
  current spike
- analyzer packaging and validation expectations are explicit in the sprint
  plans
- analyzer sample coverage and packaged-consumer validation are explicit in the
  sprint plans
- CI package-consumer validation and GitHub Packages publication expectations
  are explicit in the sprint plans
- the CLI baseline no longer treats the current implementation as an approved
  design
- the execution rules explicitly prefer deleting and replacing noncompliant
  spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets, named types, and validation
  commands to drive implementation directly

This acceptance block is the project-plan mirror of
`docs/phase-A/plan-phase-A.md` Section 8.
