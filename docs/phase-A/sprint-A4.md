---
id: A4
<<<<<<< HEAD
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
=======
title: Packaging and CLI baseline correction
status: planned
branch: integration/phase-A
target: Roslyn.DeMagic-and-roslyn-lint
---

# Sprint A4 - Packaging And CLI Baseline Correction

## Goal

- Close the analyzer packaging path and reset the CLI design baseline so it
  matches the repository's AI-first CLI contract instead of the current spike.
- Leave the CLI with an explicit replacement-oriented architecture rather than
  an implied obligation to preserve the current command implementation.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/phase-A/sprint-A3.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/LintCommandSettingsTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`

## Required Work

- validate analyzer package shape, metadata, and release gating
- ensure repo docs do not treat the current CLI implementation as approved
- align package metadata and release automation with the approved analyzer rule
  set rather than the spike descriptions
- define the CLI requirements and architecture around a stable JSON envelope,
  typed errors, MCP-ready DTOs, and auditable command pairs
- make explicit that `Program.cs` and `LintCommand.cs` are disposable if they
  obstruct the approved CLI layering

## Acceptance Criteria

- analyzer packaging is documented and testable
- analyzer package metadata no longer describes rejected spike behavior
- the CLI baseline is aligned to the approved AI-first contract rules
- the repo no longer implies the current CLI spike is the accepted design
- noncompliant CLI spike structure is planned for replacement, not preservation

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release --no-build`
- `git diff --check`
>>>>>>> f9fe54d (Finalize phase A planning framework)
