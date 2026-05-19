---
id: A10
title: analyzer sample corpus and rule matrix
status: complete
branch: sprint/A10
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A10
target: integration/phase-A
---

# Sprint A10 - Analyzer Sample Corpus And Rule Matrix

## Goal

- Build the full analyzer sample corpus required for production testing.
- Add explicit traceability from PRD rule checklists to automated samples.
- Make analyzer coverage auditable without reading implementation code.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A9.md`

## Exact Targets

- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/configuration/DeMagicConfigLoaderTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/PublicConstOutsideDesignatedFile.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/InternalConstOutsideDesignatedFile.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/DesignatedFileCompliantConst.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/DesignatedClassMismatch.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/PrivateProtectedIgnored.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/LocalConstIgnored.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/SuppressedConst.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/MissingConfigNoDiagnostics.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/SeverityFromConfig.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/ExactMatchConstField.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/PrefixMethodArgument.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/SuffixComparison.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/SubstringReturnValue.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/AttributeArgument.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/InterpolatedHole.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/NonMatchingLiteral.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/CaseSensitiveMismatch.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/SwitchArmLiteral.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/CommentsAndDocumentationIgnored.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/SuppressedLiteral.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/MissingConfigNoDiagnostics.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/InvalidConfigNoDiagnostics.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/SeverityFromConfig.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/README.md`
- `tests/sc.lint.roslyn.demagic.tests/TestMatrix.md`
- `tests/sc.lint.roslyn.demagic.tests/testing/ExpectedDiagnostic.cs`
- `tests/sc.lint.roslyn.demagic.tests/testing/RequirementTraceabilityRow.cs`
- `tests/sc.lint.roslyn.demagic.tests/testing/AnalyzerSampleKind.cs`

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
  `tests/sc.lint.roslyn.demagic.tests/testing/` so sample expectations and traceability
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

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
