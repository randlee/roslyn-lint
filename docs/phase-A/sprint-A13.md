---
id: A13
title: ci publish and manual release handoff
status: complete
branch: sprint/A13
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A13
target: integration/phase-A
---

# Sprint A13 - CI Publish And Manual Release Handoff

## Goal

- Add CI enforcement for the packaged-consumer validation path.
- Publish repo-produced packages to GitHub Packages from CI.
- Keep NuGet.org publication manual for the first release and document that
  handoff explicitly.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A12.md`
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- `docs/phase-A/production-readiness-checklist.md`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `eng/validate-sc-lint-roslyn-demagic-package.sh`
- `eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `docs/releasing.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`

## Required Work

- treat A12 closure as a hard closeout prerequisite, not a start prerequisite:
  A13 work may begin before A12 QA closes, but A13 may not close, PASS, or
  merge as complete until A12 gate artifacts are closed under their own rules
  and A13 has revalidated against that baseline
- add CI steps that pack `sc.lint.roslyn.demagic`, restore the example consumer, and
  run the cross-platform package-validation scripts
- require CI and publish flows to use the same authoritative A12 gate
  artifacts, with verification mode called out explicitly:
  `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` and
  `docs/phase-A/production-readiness-checklist.md` remain human-audit gate
  inputs that must be reviewed closed on the branch under their own internal
  rules before A13 can PASS or merge; CI and publish automation must machine-
  verify `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`,
  `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`, and
  `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md` through the same package
  validation path used locally
- confirm and apply the documented `sc.lint.roslyn` packaging strategy:
  `PackAsTool`, command name `sc-lint-roslyn`, per
  `docs/sc-lint-roslyn/architecture.md` Section 12 and `REQ-CLI-PACK-001`
- configure GitHub Packages publication for `sc.lint.roslyn.demagic`,
  `sc.lint.roslyn`, and any package dependencies produced by this repo
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
  complete, using the same machine-verifiable A12 gate artifacts and closure
  evidence consumed locally, while `PermutationMatrix.md` and
  `production-readiness-checklist.md` are explicitly reviewed as human-audit
  closure artifacts on the branch
- the `sc.lint.roslyn` packaging model is explicit in docs and matches the
  pack/publish workflow
- CI can publish repo-produced packages to GitHub Packages without changing the
  package-consumer contract
- `docs/releasing.md` is sufficient for a maintainer to perform the first
  manual NuGet.org publication safely
- package-validation scripts, manifest, and workflows describe one consistent
  release path

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `dotnet pack src/sc.lint.roslyn/sc.lint.roslyn.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-sc-lint-roslyn-demagic-package.sh`
- `pwsh -File eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `git diff --check`
