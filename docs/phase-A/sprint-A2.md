---
id: A2
title: DM002 forbidden-pattern analyzer
status: complete
branch: sprint/A2
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A2
target: integration/phase-A
---

# Sprint A2 - DM002 Forbidden-Pattern Analyzer

## Goal

- Replace the current generic string-literal spike with the approved `DM002`
  forbidden-pattern rule.
- Integrate the A1 config and pattern foundation into a real analyzer with
  fixture-backed tests and release metadata alignment.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A1.md`

## Exact Targets

- `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/analyzers/MagicStringAnalyzer.cs` deleted
- `src/sc.lint.roslyn.demagic/diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/sc.lint.roslyn.demagic/patterns/IForbiddenPatternCompiler.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPattern.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPatternKind.cs`
- `src/sc.lint.roslyn.demagic/patterns/CompiledForbiddenPattern.cs`
- `src/sc.lint.roslyn.demagic/patterns/ForbiddenPatternMatcher.cs`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `src/sc.lint.roslyn/commands/LintCommand.cs`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/MagicStringAnalyzerTests.cs` deleted
- `tests/sc.lint.roslyn.demagic.tests/testing/AnalyzerTestHarness.cs`
- `tests/sc.lint.roslyn.demagic.tests/testing/TestAdditionalText.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/*`

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

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
