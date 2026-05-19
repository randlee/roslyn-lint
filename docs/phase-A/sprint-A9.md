---
id: A9
title: DM001 completion and rule parity
status: complete
branch: sprint/A9
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A9
target: integration/phase-A
---

# Sprint A9 - DM001 Completion And Rule Parity

## Goal

- Implement the missing `DM001` constant-consolidation analyzer behavior.
- Bring the analyzer package to actual rule parity with the approved PRD.
- Remove the current mismatch where `DM001` appears in metadata but has no live
  diagnostics.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A3.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`

## Exact Targets

- `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/configuration/DeMagicConfig.cs`
- `src/sc.lint.roslyn.demagic/configuration/Dm001Options.cs`
- `src/sc.lint.roslyn.demagic/configuration/DeMagicConfigLoader.cs`
- `src/sc.lint.roslyn.demagic/diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/PublicConstOutsideDesignatedFile.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/InternalConstOutsideDesignatedFile.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/DesignatedFileCompliantConst.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/DesignatedClassMismatch.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/PrivateProtectedIgnored.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/LocalConstIgnored.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/SuppressedConst.cs`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `IDeMagicConfigLoader`, `IAdditionalFileConfigSelector`
- immutable payload types:
  `DeMagicConfig`, `Dm001Options`, `AdditionalFileConfigSelection`
- enums:
  `ConfiguredSeverity`

## Required Work

- register real Roslyn analysis actions for `DM001`
- diagnose only `public` and `internal` type-member `const` fields
- continue excluding `private`, `protected`, `private protected`, and
  `protected internal` constants in A9 because the approved sprint scope is
  still limited to explicitly `public` and `internal` declarations
- use `designated_file` as the primary location rule
- require `designated_class` when configured
- compare configured `designated_file` and `designated_class` values using one
  consistent case-insensitive policy so the rule does not drift by config key
- exclude private, protected, and local constants
- keep missing or invalid config fail-closed
- honor configured severity instead of hard-coding effective rule behavior
- update analyzer release metadata so `DM001` is no longer described as
  unimplemented when the sprint is complete
- land the first concrete `DM001` sample files needed for implementation and
  baseline behavior verification before A10 expands the full corpus

## Acceptance Criteria

- `DM001` reports diagnostics for out-of-place `public` and `internal`
  constants
- `DM001` does not report for compliant constants, private/protected constants,
  or local constants
- `DM001` respects `designated_class` when configured
- `DM001` stays suppressible through standard Roslyn suppression mechanisms
- `DM001` test coverage exists in
  `DM001ConstantConsolidationAnalyzerTests.cs` with fixture-backed samples
- the initial `DM001` sample files exist for the core positive and negative
  cases that A10 will later expand into the full production-testing corpus

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
