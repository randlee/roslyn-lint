---
id: A3
title: DM001 requirements convergence
status: planned
branch: sprint/A3
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A3
target: integration/phase-A
---

# Sprint A3 - DM001 Requirements Convergence

## Goal

- Replace the current numeric-literal spike with the approved `DM001`
  constant-consolidation rule.
- Reuse the A1/A2 configuration and descriptor foundation instead of
  preserving generic numeric-literal behavior.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A2.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Configuration/IAdditionalFileConfigSelector.cs`
- `src/Roslyn.DeMagic/Configuration/ITomlConfigParser.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/Dm001Options.cs`
- `src/Roslyn.DeMagic/Configuration/Dm002Options.cs`
- `src/Roslyn.DeMagic/Configuration/ConfiguredSeverity.cs`
- `src/Roslyn.DeMagic/Configuration/AdditionalFileConfigSelection.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicNumberAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Configuration/DeMagicConfigLoaderTests.cs`
- `tests/Roslyn.DeMagic.Tests/Configuration/ConfiguredSeverityTests.cs`

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `IAdditionalFileConfigSelector`, `ITomlConfigParser`
- immutable config payload types:
  `DeMagicConfig`, `Dm001Options`, `Dm002Options`,
  `AdditionalFileConfigSelection`
- enums:
  `ConfiguredSeverity`

## Required Work

- delete or retire the current `MagicNumberAnalyzer` spike if it obstructs the
  approved `DM001` design
- implement designated-file and optional designated-class checks against
  public/internal const field declarations
- keep local constants and private/protected constants out of scope
- load config from `AdditionalFiles` once per compilation start and reuse the
  existing immutable config payloads
- align `DM001` descriptor wording and category with constant consolidation,
  not generic magic-value detection

## Acceptance Criteria

- `DM001` diagnoses only the rule scope defined in the PRD
- `DM001` no longer reports generic numeric literal usage
- designated-file and designated-class behavior are covered by tests
- current numeric-literal spike semantics are no longer treated as correct
  behavior

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
