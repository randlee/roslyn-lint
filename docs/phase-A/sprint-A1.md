---
id: A1
title: sc.lint.roslyn.demagic analyzer foundation
status: complete
branch: sprint/A1
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A1
target: integration/phase-A
qa_fix_commit: f0668e0
---

# Sprint A1 - sc.lint.roslyn.demagic Analyzer Foundation

## Goal

- Establish the reusable foundation for `sc.lint.roslyn.demagic` before rule-specific
  implementation work in A2 and A3.
- Add configuration loading, pattern compilation, and a basic analyzer test
  harness without preserving rejected spike semantics.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A0.md`

## Exact Targets

- `src/sc.lint.roslyn.demagic/configuration/IAdditionalFileConfigSelector.cs`
- `src/sc.lint.roslyn.demagic/configuration/AdditionalFileConfigSelector.cs`
- `src/sc.lint.roslyn.demagic/configuration/ITomlConfigParser.cs`
- `src/sc.lint.roslyn.demagic/configuration/IDeMagicConfigLoader.cs`
- `src/sc.lint.roslyn.demagic/configuration/SimpleTomlConfigParser.cs`
- `src/sc.lint.roslyn.demagic/configuration/DeMagicConfig.cs`
- `src/sc.lint.roslyn.demagic/configuration/Dm001Options.cs`
- `src/sc.lint.roslyn.demagic/configuration/Dm002Options.cs`
- `src/sc.lint.roslyn.demagic/configuration/ConfiguredSeverity.cs`
- `src/sc.lint.roslyn.demagic/configuration/AdditionalFileConfigSelection.cs`
- `src/sc.lint.roslyn.demagic/configuration/DeMagicConfigLoader.cs`
- `src/sc.lint.roslyn.demagic/patterns/IForbiddenPatternCompiler.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPattern.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPatternKind.cs`
- `src/sc.lint.roslyn.demagic/patterns/CompiledForbiddenPattern.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPatternMatcher.cs`
- `tests/sc.lint.roslyn.demagic.tests/configuration/DeMagicConfigLoaderTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/configuration/ConfiguredSeverityTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/patterns/ForbiddenPatternMatcherTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/patterns/ForbiddenPatternCompilerTests.cs`

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

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
