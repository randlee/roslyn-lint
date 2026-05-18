---
id: A9
title: DM001 completion and rule parity
status: planned
branch: sprint/A9
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A9
target: integration/phase-A
---

# Sprint A9 - DM001 Completion And Rule Parity

## Goal

- Implement the missing `DM001` constant-consolidation analyzer behavior.
- Bring the analyzer package to actual rule parity with the approved PRD.
- Remove the current mismatch where `DM001` appears in metadata but has no live
  diagnostics.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A3.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/Dm001Options.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/*`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `IDeMagicConfigLoader`
- immutable payload types:
  `DeMagicConfig`, `Dm001Options`, `AdditionalFileConfigSelection`
- enums:
  `ConfiguredSeverity`

## Required Work

- register real Roslyn analysis actions for `DM001`
- diagnose only `public` and `internal` type-member `const` fields
- use `designated_file` as the primary location rule
- require `designated_class` when configured
- exclude private, protected, and local constants
- keep missing or invalid config fail-closed
- honor configured severity instead of hard-coding effective rule behavior
- update analyzer release metadata so `DM001` is no longer described as
  unimplemented when the sprint is complete

## Acceptance Criteria

- `DM001` reports diagnostics for out-of-place `public` and `internal`
  constants
- `DM001` does not report for compliant constants, private/protected constants,
  or local constants
- `DM001` respects `designated_class` when configured
- `DM001` stays suppressible through standard Roslyn suppression mechanisms
- `DM001` test coverage exists in
  `DM001ConstantConsolidationAnalyzerTests.cs` with fixture-backed samples

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
