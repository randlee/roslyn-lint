---
id: A2
<<<<<<< HEAD
title: Config model and DM001 implementation line
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Sprint A2 — Config Model And `DM001`

## Goal

Implement the repository-wide config selection model and align `DM001` with
the formal constant-consolidation contract.

## Hard Dependencies

- `A1` complete
- documentation framework accepted as source of truth

## Exact Targets

- `Directory.Build.props`
- `src/Roslyn.DeMagic/`
- `tests/Roslyn.DeMagic.Tests/`

## Required Work

- add `.roslyn-lint/` config file strategy
- wire `AdditionalFiles` selection for src vs test projects
- replace or refactor current `DM001` implementation to match the config-driven contract
- add tests for `DM001` and config loading behavior

## Acceptance Criteria

- `DM001` behavior matches documented scope and exclusions
- missing or malformed config fails safely
- source/test config selection is explicit and singular
- tests prove the documented `DM001` behavior
=======
title: DM001 requirements convergence
status: planned
branch: integration/phase-A
target: Roslyn.DeMagic
---

# Sprint A2 - DM001 Requirements Convergence

## Goal

- Replace the current numeric-literal spike with a `DM001` design that matches
  the constant-consolidation rule in the PRD.
- Establish the configuration-loading and descriptor architecture for the
  analyzer package.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A1.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicNumberAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM001ConstantConsolidationAnalyzerTests.cs`

## Required Work

- delete or retire the current `MagicNumberAnalyzer` spike if it obstructs the
  approved `DM001` design
- define immutable config models for `dm001`
- load config from `AdditionalFiles` once per compilation start
- centralize descriptor ownership instead of embedding spike-era descriptors in
  analyzer classes
- implement designated-file and optional designated-class checks against const
  field declarations
- add tests for positive, negative, missing-config, and invalid-config cases

## Acceptance Criteria

- `DM001` diagnoses only the rule scope defined in the PRD
- `DM001` no longer reports generic numeric literal usage
- config-driven severity and enablement are covered by tests
- current spike semantics are no longer treated as correct behavior
- if the spike implementation shape obstructs compliance, it is deleted rather
  than adapted
>>>>>>> f9fe54d (Finalize phase A planning framework)

## Required Validation

- `dotnet restore roslyn-lint.sln`
<<<<<<< HEAD
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release`
=======
- `dotnet build src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
>>>>>>> f9fe54d (Finalize phase A planning framework)
