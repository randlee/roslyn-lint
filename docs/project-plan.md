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
| A1 | Documentation reset | Replace placeholders and unapproved assumptions with formal suite and project docs |
| A2 | `DM001` design and config path | Align constant-consolidation behavior with the PRD and configuration model |
| A3 | `DM002` and analyzer hardening | Align forbidden-string analysis, config parsing, and analyzer validation with the PRD |
| A4 | Packaging and CLI baseline correction | Finalize analyzer packaging gates and lock the CLI design baseline to AI-first contract rules |

Phase A must not treat the current CLI spike as an approved product contract.

### Phase A Implementation Inventory

Phase A implementation work is expected to touch or replace these current code
paths:

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicNumberAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicStringAnalyzerTests.cs`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/LintCommandSettingsTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

Planned replacement-oriented analyzer implementation units:

- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`

Planned replacement-oriented CLI implementation units when CLI work resumes:

- `src/Roslyn.Lint/Contracts/`
- `src/Roslyn.Lint/Operations/`
- `src/Roslyn.Lint/Serialization/`
- `src/Roslyn.Lint/Formatting/`
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
