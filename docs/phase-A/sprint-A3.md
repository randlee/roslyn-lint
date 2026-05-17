---
id: A3
title: DM002 hardening and release alignment
status: complete
branch: sprint/A3
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A3
target: integration/phase-A
---

# Sprint A3 - DM002 Hardening And Release Alignment

## Goal

- harden the `DM002` implementation line after A2 so the branch no longer
  carries spike leftovers or broken seams
- align branch documentation and release metadata with the approved `DM002`
  analyzer and the replacement-oriented `DM001` transition state

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A2.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/MagicNumberAnalyzer.cs` deleted
- `src/Roslyn.DeMagic/Configuration/IDeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicNumberAnalyzerTests.cs` deleted

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `IDeMagicConfigLoader`, `IForbiddenPatternCompiler`
- immutable config payload types:
  `DeMagicConfig`, `Dm002Options`
- enums:
  `ConfiguredSeverity`

## Required Work

- delete the remaining `MagicNumberAnalyzer` spike and its dedicated test file
- remove `MagicNumberAnalyzer` from `LintCommand` so the CLI no longer loads
  the rejected numeric-literal rule
- keep `DM001` release metadata aligned to the approved category while using a
  replacement placeholder analyzer instead of the old spike behavior
- use the injected `IForbiddenPatternCompiler` seam in `DM002` instead of
  allocating a new matcher for each literal
- wire `DM002ForbiddenStringLiteralAnalyzer` to `IDeMagicConfigLoader` rather
  than the concrete `DeMagicConfigLoader` type
- record the completed `DM002` hardening scope in this sprint document

## Acceptance Criteria

- the `MagicNumberAnalyzer` spike is removed from production code on `sprint/A3`
- `DM002` uses both injected seams: `IDeMagicConfigLoader` and
  `IForbiddenPatternCompiler`
- `DM001` release metadata no longer uses the spike category or wording
- the sprint document records `status: complete`, `branch: sprint/A3`, and the
  active `worktree:`

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
