---
id: A4
title: CLI alignment, packaging, and release gate
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Sprint A4 — CLI Alignment, Packaging, And Release Gate

## Goal

Make the CLI and package/release path faithful to the documented analyzer
contract, deleting or narrowing any misleading CLI behavior if required.

## Hard Dependencies

- `A2` complete
- `A3` complete

## Exact Targets

- `src/Roslyn.Lint/`
- `tests/Roslyn.Lint.Tests/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## Required Work

- decide whether current direct-file CLI analysis is faithful enough to keep
- refactor or replace the CLI analysis path if it is not
- align CLI output and exit behavior with the documented contract
- verify tool packaging and end-to-end release behavior for both packages

## Acceptance Criteria

- the CLI no longer contradicts the documented analyzer/config model
- tool packaging remains valid
- CI and publish workflows validate the final Phase A delivery line
- no misleading spike-only CLI path remains in the shipped product surface

## Required Validation

- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj -c Release --no-build`
- `dotnet pack src/Roslyn.Lint/Roslyn.Lint.csproj -c Release --no-build`
