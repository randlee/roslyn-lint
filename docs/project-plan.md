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
| A3 | `DM001` design and config path | Align constant-consolidation behavior with the PRD and configuration model |
| A4 | Packaging and CLI baseline correction | Finalize analyzer packaging gates and lock the CLI design baseline to AI-first contract rules |

Phase A must not treat the current CLI spike as an approved product contract.

### Phase A Implementation Inventory

Phase A implementation work is expected to touch or replace these current code
paths:

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicNumberAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/LintCommandSettingsTests.cs`
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

- `src/Roslyn.Lint/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint/Contracts/CliError.cs`
- `src/Roslyn.Lint/Contracts/CliWarning.cs`
- `src/Roslyn.Lint/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint/Contracts/LintRequest.cs`
- `src/Roslyn.Lint/Contracts/LintResult.cs`
- `src/Roslyn.Lint/Contracts/LintIssue.cs`
- `src/Roslyn.Lint/Operations/ICommandOperation.cs`
- `src/Roslyn.Lint/Operations/ILintWorkspaceAdapter.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `src/Roslyn.Lint/Adapters/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Operations/`

## 5. Execution Rules

- requirements and architecture documents are authoritative over the current
  spike implementation
- analyzer-first work takes precedence over speculative CLI feature work until
  the analyzer baseline is approved
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
- the CLI baseline no longer treats the current implementation as an approved
  design
- the execution rules explicitly prefer deleting and replacing noncompliant
  spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets, named types, and validation
  commands to drive implementation directly

This acceptance block is the project-plan mirror of
`docs/phase-A/plan-phase-A.md` Section 8.
