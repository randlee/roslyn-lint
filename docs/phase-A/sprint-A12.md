---
id: A12
title: production-readiness convergence
status: complete
branch: sprint/A12
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A12
target: integration/phase-A
---

# Sprint A12 - Production-Readiness Convergence

## Goal

- Make `Roslyn.DeMagic` ready for production testing as the primary Phase A
  deliverable.
- Align package metadata, documentation, release metadata, and
  packaged-consumer validation with the actual shipped analyzer rule set.
- Produce the readiness evidence that A13 will enforce and publish through CI.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A11.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `eng/roslyn-demagic-package-expected-diagnostics.json`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `examples/Roslyn.DeMagic.PackageSmoke/`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`
- `eng/roslyn-demagic-package-expected-diagnostics.json`
- `docs/phase-A/production-readiness-checklist.md`
- `docs/releasing.md`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/ProductionReadinessChecklistRow.cs`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`, `ProductionReadinessChecklistRow`

## Required Work

- make release metadata reflect the real implemented rule set and nothing more
- update repo documentation so analyzer readiness is described accurately and
  CLI work is not misrepresented as the primary Phase A outcome
- add a production-readiness checklist for:
  rule coverage, package consumption, metadata alignment, and documented
  validation commands
- freeze the expected-diagnostics manifest and package-validation script outputs
  to the exact shippable analyzer rule set
- define `ProductionReadinessChecklistRow` so the checklist records:
  requirement area, artifact, validation command, owner, and status evidence

## Acceptance Criteria

- docs no longer imply unimplemented analyzer behavior is production-ready
- rule metadata, release metadata, and sample corpus all describe the same
  shippable analyzer set
- the production-readiness checklist is complete and auditable
- the packaged-consumer manifest and validation scripts describe the same
  shipped diagnostics that release metadata and docs describe
- no CI publication behavior is still ambiguous when A12 closes; A13 inherits a
  finished readiness contract rather than redefining one

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-roslyn-demagic-package.sh`
- `pwsh -File eng/validate-roslyn-demagic-package.ps1`
- `git diff --check`
