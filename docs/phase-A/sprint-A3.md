---
id: A3
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
