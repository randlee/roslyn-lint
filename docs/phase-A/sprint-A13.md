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
- `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md`
- `docs/phase-A/production-readiness-checklist.md`
- `eng/roslyn-demagic-package-expected-diagnostics.json`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`

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

- treat A12 closure as a hard closeout prerequisite, not a start prerequisite:
  A13 work may begin before A12 QA closes, but A13 may not close, PASS, or
  merge as complete until A12 gate artifacts are closed under their own rules
  and A13 has revalidated against that baseline
- add CI steps that pack `Roslyn.DeMagic`, restore the example consumer, and
  run the cross-platform package-validation scripts
- require CI and publish flows to use the same authoritative A12 gate artifacts:
  `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md`,
  `docs/phase-A/production-readiness-checklist.md`,
  `eng/roslyn-demagic-package-expected-diagnostics.json`,
  `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`,
  `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
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
- do not call A13 complete if it depends on an A12 artifact that is present but
  still open under that artifact's own internal rule

## Acceptance Criteria

- A12 gate artifacts are already closed on the reviewed branch and remain
  closed after A13 changes
- CI proves the packaged analyzer-consumer path before Phase A is called
  complete, using the same A12 gate artifacts and closure evidence consumed
  locally
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
- `pwsh -File eng/validate-roslyn-demagic-package.ps1`
- `git diff --check`
