---
id: A12
title: production-readiness hardening and release gate
status: planned
branch: sprint/A12
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A12
target: integration/phase-A
---

# Sprint A12 - Production-Readiness Hardening And Release Gate

## Goal

- Make `Roslyn.DeMagic` ready for production testing as the primary Phase A
  deliverable.
- Align CI, package metadata, documentation, and packaged-consumer validation
  with the actual shipped analyzer rule set.
- Publish analyzer, CLI, and any package dependencies to GitHub Packages from
  CI while keeping NuGet.org publication manual for the first release.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A11.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `examples/Roslyn.DeMagic.PackageSmoke/`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`
- `docs/phase-A/production-readiness-checklist.md`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  release metadata rows, package-validation outputs, and readiness checklist
  records

## Required Work

- add CI gates that pack `Roslyn.DeMagic` and run the packaged-consumer
  validation path
- configure CI publication of GitHub Packages artifacts for:
  `Roslyn.DeMagic`, `Roslyn.Lint`, and any package dependencies produced by the
  repo
- keep NuGet.org publication manual in this sprint; document the manual release
  handoff and do not automate NuGet.org push yet
- make release metadata reflect the real implemented rule set and nothing more
- update repo documentation so analyzer readiness is described accurately and
  CLI work is not misrepresented as the primary Phase A outcome
- add a production-readiness checklist for:
  rule coverage, package consumption, metadata alignment, and documented
  validation commands

## Acceptance Criteria

- CI proves the analyzer package works in a normal consumer project
- CI can publish the analyzer and CLI packages to GitHub Packages
- docs no longer imply unimplemented analyzer behavior is production-ready
- rule metadata, release metadata, and sample corpus all describe the same
  shippable analyzer set
- the production-readiness checklist is complete and auditable
- NuGet.org publication remains manual but documented for the first release

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/Roslyn.Lint/Roslyn.Lint.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-roslyn-demagic-package.sh`
- `git diff --check`
