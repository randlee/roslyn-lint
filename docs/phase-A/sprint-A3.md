---
id: A3
<<<<<<< HEAD
title: DM002 and analyzer contract alignment
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Sprint A3 — `DM002` And Analyzer Contract Alignment

## Goal

Implement the formal forbidden-string-literal contract for `DM002` and align
the analyzer package metadata with the documented delivery line.

## Hard Dependencies

- `A2` complete

## Exact Targets

- `src/Roslyn.DeMagic/`
- `tests/Roslyn.DeMagic.Tests/`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`

## Required Work

- replace or refactor current `DM002` implementation to support exact/prefix/suffix/substring matching
- align diagnostic category and default severity with the formal requirement
- ensure analyzer release tracking files reflect the real rule surface
- add tests for pattern matching, exclusions, and case-sensitivity behavior

## Acceptance Criteria

- `DM002` matches the documented pattern model
- diagnostic metadata matches the formal contract
- analyzer release tracking is updated consistently
- tests prove documented `DM002` behavior

## Required Validation

- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj -c Release --no-build`
=======
title: DM002 and analyzer hardening
status: planned
branch: integration/phase-A
target: Roslyn.DeMagic
---

# Sprint A3 - DM002 And Analyzer Hardening

## Goal

- Implement the forbidden-string rule from the PRD and harden the analyzer
  configuration and validation path.
- Replace the current generic string-literal spike with compiled forbidden-
  pattern analysis.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-demagic/boundaries.md`
- `docs/phase-A/sprint-A2.md`

## Exact Targets

- `src/Roslyn.DeMagic/Analyzers/MagicStringAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/Patterns/ForbiddenPatternMatcher.cs`
- `src/Roslyn.DeMagic/Configuration/DeMagicConfig.cs`
- `src/Roslyn.DeMagic/Diagnostics/DeMagicDiagnosticDescriptors.cs`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `tests/Roslyn.DeMagic.Tests/Analyzers/MagicStringAnalyzerTests.cs`
- `tests/Roslyn.DeMagic.Tests/Analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`

## Required Work

- delete or retire the current `MagicStringAnalyzer` spike if it obstructs the
  approved `DM002` design
- extend the immutable config model for `dm002`
- implement exact, prefix, suffix, and substring forbidden-pattern matching
- support case sensitivity options
- validate graceful disablement, invalid-config, and severity behavior
- align analyzer release metadata with the actual rule set and diagnostic
  categories

## Acceptance Criteria

- `DM002` behavior matches the PRD
- pattern matching is covered by positive and negative tests
- release metadata and descriptor categories match the approved diagnostics
- spike-era generic magic-string semantics are no longer part of the product

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `git diff --check`
>>>>>>> f9fe54d (Finalize phase A planning framework)
