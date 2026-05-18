---
id: A11
title: packaged consumer validation
status: planned
branch: sprint/A11
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A11
target: integration/phase-A
---

# Sprint A11 - Packaged Consumer Validation

## Goal

- Prove the built `Roslyn.DeMagic` package works from a normal consuming
  project.
- Validate the analyzer as a package, not only as an in-repo project reference.
- Create the production-testing fixture that real users can run locally.

## Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A10.md`

## Exact Targets

- `examples/Roslyn.DeMagic.PackageSmoke/NuGet.config`
- `examples/Roslyn.DeMagic.PackageSmoke/Directory.Build.props`
- `examples/Roslyn.DeMagic.PackageSmoke/Roslyn.DeMagic.PackageSmoke.csproj`
- `examples/Roslyn.DeMagic.PackageSmoke/.roslyn-lint/config-src.toml`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/*`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/*`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  sample projects, local-feed package references, expected-diagnostic fixtures

## Required Work

- pack `Roslyn.DeMagic` from the repo build into a local package output
- configure a local NuGet feed for the example consumer project
- reference `Roslyn.DeMagic` by package reference, not by project reference
- add example source files that intentionally trigger every analyzer rule
- add compliant and suppression examples so the package consumer can verify both
  diagnostic presence and absence
- provide cross-platform validation scripts that pack, restore, build, and
  assert expected diagnostics from the consumer project

## Acceptance Criteria

- the example project restores `Roslyn.DeMagic` from a locally built package
- the example project emits expected diagnostics for `DM001` and `DM002`
- compliant and suppressed samples behave as expected when the package is
  consumed normally
- package-consumer validation does not rely on in-repo project references or
  special test-only analyzer wiring

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-roslyn-demagic-package.sh`
- `git diff --check`
