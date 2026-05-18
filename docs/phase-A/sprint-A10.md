---
id: A10
title: analyzer sample corpus and rule matrix
status: complete
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
- `tests/Roslyn.DeMagic.Tests/Configuration/DeMagicConfigLoaderTests.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/PublicConstOutsideDesignatedFile.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/InternalConstOutsideDesignatedFile.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/DesignatedFileCompliantConst.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/DesignatedClassMismatch.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/PrivateProtectedIgnored.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/LocalConstIgnored.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/SuppressedConst.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/MissingConfigNoDiagnostics.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM001/SeverityFromConfig.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/ExactMatchConstField.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/PrefixMethodArgument.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/SuffixComparison.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/SubstringReturnValue.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/AttributeArgument.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/InterpolatedHole.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/NonMatchingLiteral.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/CaseSensitiveMismatch.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/SwitchArmLiteral.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/CommentsAndDocumentationIgnored.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/SuppressedLiteral.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/MissingConfigNoDiagnostics.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/InvalidConfigNoDiagnostics.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/DM002/SeverityFromConfig.cs`
- `tests/Roslyn.DeMagic.Tests/TestData/README.md`
- `tests/Roslyn.DeMagic.Tests/TestMatrix.md`
- `tests/Roslyn.DeMagic.Tests/Testing/ExpectedDiagnostic.cs`
- `tests/Roslyn.DeMagic.Tests/Testing/RequirementTraceabilityRow.cs`
- `tests/Roslyn.DeMagic.Tests/Testing/AnalyzerSampleKind.cs`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedDiagnostic`, `RequirementTraceabilityRow`
- enums:
  `AnalyzerSampleKind`

## Required Work

- add positive and negative `DM001` samples for:
  designated-file mismatch, designated-class mismatch, compliant constants,
  private/protected constants, local constants, suppression, missing config,
  and severity-from-config behavior
- record the v1 `DM001` visibility boundary explicitly: `private`,
  `protected`, `private protected`, and `protected internal` constants remain
  out of scope
- expand `DM002` samples so the PRD checklist is represented directly in test
  data, including suppression, missing config, invalid config, and
  severity-from-config behavior
- add suppression samples for both `DM001` and `DM002`
- add corner-case samples for both rules without leaving any PRD checkbox
  covered only by prose
- create a matrix document that maps every PRD rule checkbox to one or more
  automated tests or sample files
- define `AnalyzerSampleKind` so sample ownership is explicit:
  positive, negative, suppression, config-failure, severity, or corner-case
- define `RequirementTraceabilityRow` so `TestMatrix.md` records:
  requirement id, rule id, sample file, owning test method, and validation mode
- define the compiled test-support types above under
  `tests/Roslyn.DeMagic.Tests/Testing/` so sample expectations and traceability
  rows have one explicit project home
- add explicit config-loader coverage for source/test config routing, missing
  config disablement, invalid-config error reporting, and config independence
  without merging or inheritance between source and test config files

## Acceptance Criteria

- every approved analyzer rule has positive, negative, and suppression samples
- every config behavior promised by the PRD has at least one concrete automated
  sample
- source/test config routing and no-merge independence are verified directly in
  `DeMagicConfigLoaderTests`
- `DM001` and `DM002` PRD checklists are traceable to concrete automated tests
- the sample corpus is readable as documentation for production testing
- `TestMatrix.md` is sufficient for a reviewer to verify coverage without
  inferring unstated cases

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
