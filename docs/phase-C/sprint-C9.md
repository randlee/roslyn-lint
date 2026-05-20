---
id: C9
title: boundary package ci and release hardening
status: planned
branch: sprint/C9
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/C9
target: integration/phase-C
---

# Sprint C9 - Boundary Package CI And Release Hardening

## Goal

- Deliver CI, packaging, and release hardening only for `sc-lint-roslyn-boundary`.

## Hard Dependencies

- `docs/phase-C/sprint-C8.md`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `src/sc.lint.roslyn.boundary/sc.lint.roslyn.boundary.csproj`
- `docs/releasing.md`
- `docs/sc-lint-roslyn-boundary/requirements.md`
- `docs/sc-lint-roslyn-boundary/architecture.md`

## Required Work

- add CI validation for the boundary package
- add packaging and publication steps for `sc-lint-roslyn-boundary`
- document the release path and operator validation steps
- ensure CI and publish flows use the same package shape and validation path
- do not add new rule families, graph features, or dogfooding findings capture
  in this sprint

## Acceptance Criteria

- CI validates the boundary package deterministically
- publish/release flow for the boundary package is documented and wired
- package shape and validation path are consistent across local and CI usage
- no new boundary-analysis feature scope is mixed into this sprint

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `git diff --check`
