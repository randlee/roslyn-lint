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
- a dedicated CLI contract document for `roslyn-lint`
- project-level boundary inventories for both projects
- accepted repository ADRs for enforceable Phase A decisions
- a sprinted Phase A plan
- a PRD-aligned `Roslyn.DeMagic` v1 implementation
- a CLI design baseline aligned with the repository's AI-first CLI rules
- a usable top-level `roslyn-lint` CLI for the first suite workflows

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
- `docs/roslyn-lint/cli-contract.md`
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
| A4 | CLI orchestration planning reset | Replace the ad hoc CLI baseline with a `creating-ai-clis` and `sc-lint`-aligned orchestration plan |
| A5 | CLI foundation and abstractions package | Replace Spectre with `System.CommandLine`, add `Roslyn.Lint.Abstractions`, and ship working `version` and `view tools` commands |
| A6 | DeMagic backend integration and first usable lint flow | Implement `lint roslyn-demagic` through the shared dispatch and contract seams |
| A7 | Profiles plus check, clippy, and ci workflows | Implement real local workflow commands using `.NET`-native tooling |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Implement stable `view` targets and harden the multi-tool module model |

Phase A must not treat the current CLI spike as an approved product contract.

### Phase A Implementation Inventory

Phase A implementation work is expected to touch, add, delete, or replace
these code paths:

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs` deleted in A3
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint.Abstractions/`
- `src/Roslyn.Lint/Commands/`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Abstractions/`
- `tests/Roslyn.Lint.Tests/Commands/`
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
- `src/Roslyn.Lint.Abstractions/Attributes/BoundaryDeclarationAttribute.cs`
- `src/Roslyn.Lint.Abstractions/Attributes/LintToolAttribute.cs`
- `src/Roslyn.Lint/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint/Contracts/CliError.cs`
- `src/Roslyn.Lint/Contracts/CliDiagnostic.cs`
- `src/Roslyn.Lint/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint/CommandModel/CommandFamily.cs`
- `src/Roslyn.Lint/CommandModel/LintProfile.cs`
- `src/Roslyn.Lint/CommandModel/OutputMode.cs`
- `src/Roslyn.Lint/CommandModel/BackendExecutionMode.cs`
- `src/Roslyn.Lint/Contracts/LintToolRequest.cs`
- `src/Roslyn.Lint/Contracts/LintToolResult.cs`
- `src/Roslyn.Lint/Contracts/LintFinding.cs`
- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint/Contracts/CheckRequest.cs`
- `src/Roslyn.Lint/Contracts/CheckResult.cs`
- `src/Roslyn.Lint/Contracts/ClippyRequest.cs`
- `src/Roslyn.Lint/Contracts/ClippyResult.cs`
- `src/Roslyn.Lint/Contracts/CiRequest.cs`
- `src/Roslyn.Lint/Contracts/CiResult.cs`
- `src/Roslyn.Lint/Contracts/VersionResult.cs`
- `src/Roslyn.Lint/Dispatch/BackendToolDescriptor.cs`
- `src/Roslyn.Lint/Dispatch/IBackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/IBackendProcessRunner.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Operations/ILintToolOperation.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/ICheckOperation.cs`
- `src/Roslyn.Lint/Operations/IClippyOperation.cs`
- `src/Roslyn.Lint/Operations/ICiOperation.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Serialization/RoslynLintJsonContext.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Commands/RegisterVersionCommand.cs`
- `src/Roslyn.Lint/Backends/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Dispatch/`
- `tests/Roslyn.Lint.Tests/Operations/`
- `tests/Roslyn.Lint.Tests/Commands/`
- `README.md`

## 5. Execution Rules

- requirements and architecture documents are authoritative over the current
  spike implementation
- analyzer-first work takes precedence over speculative CLI feature work until
  the analyzer baseline is approved
- future CLI implementation must inherit the contract rules defined in
  `docs/roslyn-lint/requirements.md`, `docs/roslyn-lint/architecture.md`, and
  `docs/roslyn-lint/cli-contract.md`
- future CLI implementation must use the local `creating-ai-clis` skill as the
  design authority and mirror the `sc-lint` top-level command-family pattern
  unless a later ADR changes that rule explicitly
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
- the CLI baseline no longer treats the current implementation as an approved
  design
- the CLI baseline defines the top-level command families, envelope, command
  identifiers, backend-dispatch seams, and planned file inventory explicitly
- the CLI is implemented enough to support immediate repo use through the first
  stable command set
- the execution rules explicitly prefer deleting and replacing noncompliant
  spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets, named types, and validation
  commands to drive implementation directly

This acceptance block is the project-plan mirror of
`docs/phase-A/plan-phase-A.md` Section 8.
