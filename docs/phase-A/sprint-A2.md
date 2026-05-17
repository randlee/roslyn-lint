---
id: A2
title: Config model and DM001 implementation line
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Sprint A2 — Config Model And `DM001`

## Goal

Implement the repository-wide config selection model and align `DM001` with
the formal constant-consolidation contract.

## Hard Dependencies

- `A1` complete
- documentation framework accepted as source of truth

## Exact Targets

- `Directory.Build.props`
- `src/Roslyn.DeMagic/`
- `tests/Roslyn.DeMagic.Tests/`

## Required Work

- add `.roslyn-lint/` config file strategy
- wire `AdditionalFiles` selection for src vs test projects
- replace or refactor current `DM001` implementation to match the config-driven contract
- add tests for `DM001` and config loading behavior

## Acceptance Criteria

- `DM001` behavior matches documented scope and exclusions
- missing or malformed config fails safely
- source/test config selection is explicit and singular
- tests prove the documented `DM001` behavior

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release`
