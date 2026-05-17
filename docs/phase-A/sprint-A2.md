---
id: A2
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

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
