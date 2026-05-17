---
id: A2
title: DM002 forbidden-pattern analyzer
status: complete
branch: sprint/A2
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A2
target: integration/phase-A
---

# Sprint A2 - DM002 Forbidden-Pattern Analyzer

## Goal

- Replace the current generic string-literal spike with the approved `DM002`
  forbidden-pattern rule.
- Integrate the A1 config and pattern foundation into a real analyzer with
  fixture-backed tests and release metadata alignment.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A1.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs` deleted
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/Patterns/IForbiddenPatternCompiler.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternKind.cs`
- `src/Roslyn.DeMagic/Patterns/CompiledForbiddenPattern.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicStringAnalyzerTests.cs` deleted
- `tests/Roslyn.DeMagic.Tests/Testing/AnalyzerTestHarness.cs`
- `tests/Roslyn.DeMagic.Tests/Testing/TestAdditionalText.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/*`

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `IForbiddenPatternCompiler`
- immutable pattern payload types:
  `ForbiddenPattern`, `CompiledForbiddenPattern`
- enums:
  `ForbiddenPatternKind`, `ConfiguredSeverity`

## Required Work

- delete the current `MagicStringAnalyzer` spike rather than preserving generic
  magic-string behavior
- compile configured forbidden patterns once per compilation start and reuse
  them in syntax callbacks
- load `dm002` config from `AdditionalFiles` and fail closed on missing or
  invalid config
- report exact, prefix, suffix, and substring violations with the matched
  forbidden pattern in the diagnostic message
- honor configured severity without introducing a second severity vocabulary
- replace raw-string unit tests with fixture-backed analyzer tests that run
  real source files through the analyzer harness

## Acceptance Criteria

- `DM002` behavior matches the PRD across exact, prefix, suffix, and substring
  matching
- config-driven severity and enablement are covered by analyzer tests
- invalid or missing config disables `DM002` without crashing analysis
- attribute arguments and const declarations are analyzed when they match
  forbidden patterns
- interpolated string holes remain out of scope and are not diagnosed
- the generic magic-string spike is removed rather than preserved through
  compatibility edits

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
