---
id: A13
title: ci publish and manual release handoff
status: planned
branch: sprint/A13
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A13
target: integration/phase-A
---

# Sprint A13 - CI Publish And Manual Release Handoff

## Goal

- Add CI enforcement for the packaged-consumer validation path.
- Publish repo-produced packages to GitHub Packages from CI.
- Keep NuGet.org publication manual for the first release and document that
  handoff explicitly.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A12.md`
- `docs/phase-A/production-readiness-checklist.md`
- `eng/roslyn-demagic-package-expected-diagnostics.json`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`
- `eng/roslyn-demagic-package-expected-diagnostics.json`
- `docs/releasing.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`

## Required Work

- add CI steps that pack `Roslyn.DeMagic`, restore the example consumer, and
  run the cross-platform package-validation scripts
- confirm and apply the documented `Roslyn.Lint` packaging strategy:
  `PackAsTool`, command name `roslyn-lint`, per
  `docs/roslyn-lint/architecture.md` Section 12 and `REQ-CLI-PACK-001`
- configure GitHub Packages publication for `Roslyn.DeMagic`,
  `Roslyn.Lint`, and any package dependencies produced by this repo
- ensure publication credentials and package-source assumptions live in CI and
  release docs rather than in sample-project ad hoc setup
- document the manual NuGet.org first-release procedure, including exact pack,
  verify, and push steps and the point where future automation may begin
- make the publish workflow depend on the same package-validation artifacts and
  manifest checks used by local validation

## Acceptance Criteria

- CI proves the packaged analyzer-consumer path before Phase A is called
  complete
- the `Roslyn.Lint` packaging model is explicit in docs and matches the
  pack/publish workflow
- CI can publish repo-produced packages to GitHub Packages without changing the
  package-consumer contract
- `docs/releasing.md` is sufficient for a maintainer to perform the first
  manual NuGet.org publication safely
- package-validation scripts, manifest, and workflows describe one consistent
  release path

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/Roslyn.Lint/Roslyn.Lint.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-roslyn-demagic-package.sh`
- `git diff --check`
