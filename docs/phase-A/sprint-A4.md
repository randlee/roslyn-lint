---
id: A4
title: Analyzer packaging, release gate, and CLI deferral boundary
status: planned
branch: integration/phase-A
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/integration/phase-A
target: develop
---

# Sprint A4 — Analyzer Packaging, Release Gate, And CLI Deferral Boundary

## Goal

Close the analyzer packaging and release path, and explicitly defer detailed
CLI behavior until dedicated CLI requirements are available.

## Hard Dependencies

- `A2` complete
- `A3` complete

## Exact Targets

- `tests/Roslyn.Lint.Tests/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## Required Work

- verify analyzer and tool packaging on the shared release line
- align release automation with the documented analyzer-first delivery path
- keep the CLI project boundary documented without finalizing detailed CLI
  behavior prematurely

## Acceptance Criteria

- analyzer packaging is validated end to end
- tool packaging remains valid
- CI and publish workflows validate the final Phase A delivery line
- the repo no longer implies that current CLI spike behavior is the final
  contract

## Required Validation

- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj -c Release --no-build`
- `dotnet pack src/Roslyn.Lint/Roslyn.Lint.csproj -c Release --no-build`
