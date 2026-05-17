---
id: A1
title: Roslyn.DeMagic analyzer foundation
status: complete
branch: sprint/A1
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A1
target: integration/phase-A
---

# Sprint A1 - Roslyn.DeMagic Analyzer Foundation

## Goal

- Establish the reusable foundation for `Roslyn.DeMagic` before rule-specific
  implementation work in A2 and A3.
- Add configuration loading, pattern compilation, and a basic analyzer test
  harness without preserving rejected spike semantics.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A0.md`

## Exact Targets

- `src/Roslyn.DeMagic/Configuration/IAdditionalFileConfigSelector.cs`
- `src/Roslyn.DeMagic/Configuration/AdditionalFileConfigSelector.cs`
- `src/Roslyn.DeMagic/Configuration/ITomlConfigParser.cs`
- `src/Roslyn.DeMagic/Configuration/IDeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Configuration/SimpleTomlConfigParser.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Configuration/Dm001Options.cs`
- `src/Roslyn.DeMagic/Configuration/Dm002Options.cs`
- `src/Roslyn.DeMagic/Configuration/ConfiguredSeverity.cs`
- `src/Roslyn.DeMagic/Configuration/AdditionalFileConfigSelection.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfigLoader.cs`
- `src/Roslyn.DeMagic/Patterns/IForbiddenPatternCompiler.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternKind.cs`
- `src/Roslyn.DeMagic/Patterns/CompiledForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `tests/Roslyn.DeMagic.Tests/Configuration/DeMagicConfigLoaderTests.cs`
- `tests/Roslyn.DeMagic.Tests/Configuration/ConfiguredSeverityTests.cs`
- `tests/Roslyn.DeMagic.Tests/Patterns/ForbiddenPatternMatcherTests.cs`
- `tests/Roslyn.DeMagic.Tests/Patterns/ForbiddenPatternCompilerTests.cs`

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `IAdditionalFileConfigSelector`, `ITomlConfigParser`,
  `IDeMagicConfigLoader`, `IForbiddenPatternCompiler`
- immutable configuration payload types:
  `DeMagicConfig`, `Dm001Options`, `Dm002Options`,
  `AdditionalFileConfigSelection`
- immutable pattern payload types:
  `ForbiddenPattern`, `CompiledForbiddenPattern`
- enums:
  `ConfiguredSeverity`, `ForbiddenPatternKind`

## Required Work

- add an `AdditionalFiles` configuration selection seam for analyzer config
  payloads
- add a minimal TOML parser that supports the current `dm001` / `dm002` schema
- add immutable configuration models for the analyzer package
- add forbidden-pattern compilation and matching infrastructure
- add sample-code and configuration tests for the new foundation layer
- keep config loading fail-closed and fully seamable for later analyzer wiring
- keep the current solution building while introducing the foundation types

## Acceptance Criteria

- the configuration system loads `.toml` settings through the shared
  `DeMagicConfigLoader`
- the forbidden-pattern matcher produces expected results for exact, prefix,
  suffix, and substring patterns
- the new configuration and pattern tests pass
- the solution builds cleanly after the foundation layer is added
- this sprint doc records `status: complete`, `branch: sprint/A1`, and the
  active `worktree:`

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
