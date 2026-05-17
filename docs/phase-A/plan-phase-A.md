# Phase A Plan

## 1. Goal

Phase A establishes the repository's formal documentation baseline and delivers
the first approved implementation line for `Roslyn.DeMagic`.

The CLI is still part of the suite, but Phase A treats it as a contract-first
design line rather than assuming the current spike is valid.

## 2. Deliverables

- suite-level documentation framework
- project-level requirements and architecture for both products
- project-level boundary inventories for both products
- PRD-aligned `Roslyn.DeMagic` design and implementation plan
- AI-first `roslyn-lint` CLI baseline
- packaging and validation gates for the analyzer

## 3. Execution Branch

- branch: `integration/phase-A`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/roslyn-demagic/*`
- `docs/roslyn-lint/*`
- `.claude/skills/creating-ai-clis/`

## 5. Exact Implementation Targets

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs`
- `src/Roslyn.DeMagic/Configuration/`
- `src/Roslyn.DeMagic/Diagnostics/`
- `src/Roslyn.DeMagic/Patterns/`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Contracts/`
- `src/Roslyn.Lint/Operations/`
- `src/Roslyn.Lint/Serialization/`
- `src/Roslyn.Lint/Formatting/`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Operations/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| A1 | Documentation reset | Replace placeholders and unapproved assumptions with approved suite and project docs |
| A2 | `DM001` requirements convergence | Replace the numeric-literal spike with config-driven constant-consolidation analysis |
| A3 | `DM002` and analyzer hardening | Replace the generic string-literal spike with forbidden-pattern analysis and aligned release metadata |
| A4 | Packaging and CLI baseline correction | Finalize analyzer package and release gates and define the replacement-oriented CLI baseline |

## 7. Implementation Strategy

- A1 documents the approved target and replacement policy
- A2 deletes the `DM001` spike behavior and introduces the real config and
  declaration-analysis path
- A3 deletes the `DM002` spike behavior and introduces compiled forbidden-
  pattern matching and analyzer metadata alignment
- A4 validates analyzer package outputs and leaves the CLI with a strict design
  baseline that future implementation must follow
- no sprint in Phase A should preserve current spike semantics merely because
  code already exists

### 7.1 Named Type Inventory

Phase A planning now assumes these implementation types will exist or be
introduced during the development sprints:

- analyzer configuration and boundary types:
  `IAdditionalFileConfigSelector`, `ITomlConfigParser`,
  `DeMagicConfig`, `Dm001Options`, `Dm002Options`,
  `AdditionalFileConfigSelection`, `ConfiguredSeverity`,
  `DeMagicConfigLoader`
- analyzer pattern types:
  `IForbiddenPatternCompiler`, `ForbiddenPattern`,
  `CompiledForbiddenPattern`, `ForbiddenPatternKind`,
  `ForbiddenPatternMatcher`
- CLI contract and boundary types:
  `ICommandOperation<TRequest, TResponse>`, `ILintWorkspaceAdapter`,
  `IJsonEnvelopeWriter`, `IHumanOutputFormatter<TResponse>`,
  `CliEnvelope<TResult>`, `CliError`, `CliWarning`,
  `LintRequest`, `LintResult`, `LintIssue`, `CliErrorKind`

## 8. Acceptance

Phase A is complete only when:

- the repo documentation framework exists and is internally consistent
- `Roslyn.DeMagic` behavior matches the PRD rather than the current spike
- analyzer packaging and tests reflect the approved diagnostic set
- the CLI baseline no longer treats the current implementation as an approved
  design
- the Phase A execution rules explicitly prefer deleting and replacing
  noncompliant spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets and validation commands to drive
  implementation directly
