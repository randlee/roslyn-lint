---
id: A10
title: analyzer sample corpus and rule matrix
status: planned
branch: sprint/A10
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A10
target: integration/phase-A
---

# Sprint A10 - Analyzer Sample Corpus And Rule Matrix

## Goal

- Build the full analyzer sample corpus required for production testing.
- Add explicit traceability from PRD rule checklists to automated samples.
- Make analyzer coverage auditable without reading implementation code.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A9.md`

## Exact Targets

- `tests/Roslyn.DeMagic.Tests/Analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/*`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/*`
- `tests/Roslyn.DeMagic.Tests/TestData/README.md`
- `tests/Roslyn.DeMagic.Tests/TestMatrix.md`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  analyzer sample source files, expected-diagnostic fixtures, and requirement
  traceability rows

## Required Work

- add positive and negative `DM001` samples for:
  designated-file mismatch, designated-class mismatch, compliant constants,
  private/protected constants, and local constants
- expand `DM002` samples so the PRD checklist is represented directly in test
  data
- add suppression samples for both `DM001` and `DM002`
- add corner-case samples for both rules, including config-driven severity and
  missing/invalid config behavior
- create a matrix document that maps every PRD rule checkbox to one or more
  automated tests or sample files

## Acceptance Criteria

- every approved analyzer rule has positive, negative, and suppression samples
- `DM001` and `DM002` PRD checklists are traceable to concrete automated tests
- the sample corpus is readable as documentation for production testing
- `TestMatrix.md` is sufficient for a reviewer to verify coverage without
  inferring unstated cases

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
