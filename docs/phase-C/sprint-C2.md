---
id: C2
title: boundary package scaffold
status: planned
branch: sprint/C2
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C2
target: integration/phase-C
---

# Sprint C2 - Boundary Package Scaffold

## Goal

- Create the `sc-lint-roslyn-boundary` package and top-level command shell only.

## Hard Dependencies

- `docs/phase-C/boundary-package-plan.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`

## Exact Targets

- `src/sc.lint.roslyn.boundary/`
- `src/sc.lint.roslyn.boundary/sc.lint.roslyn.boundary.csproj`
- `src/sc.lint.roslyn.boundary/Program.cs`
- `src/sc.lint.roslyn.boundary/Commands/`
- `src/sc.lint.roslyn.boundary/Contracts/`
- `tests/sc.lint.roslyn.boundary.tests/`
- `tests/sc.lint.roslyn.boundary.tests/sc.lint.roslyn.boundary.tests.csproj`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- create the package and project structure for `sc-lint-roslyn-boundary`
- define the top-level command shell only:
  `analyze`, `export-graph`, and `version`
- wire the new package into the solution and repo build
- create placeholder contract types and test assembly structure only as needed
  to support the scaffold
- do not implement config loading, graph extraction, export, or rule logic in
  this sprint

## Acceptance Criteria

- the new package exists and builds as part of the solution
- the top-level command shell exists with placeholder behavior only
- the package has owned requirements and architecture docs
- no config loader, graph extraction, export, or lint-rule implementation is
  mixed into this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
