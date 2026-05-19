---
id: A12
title: production-readiness convergence
status: complete
branch: sprint/A12
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A12
target: integration/phase-A
---

# Sprint A12 - Production-Readiness Convergence

## Goal

- Make `sc.lint.roslyn.demagic` ready for production testing as the primary Phase A
  deliverable.
- Align package metadata, documentation, release metadata, and
  packaged-consumer validation with the actual shipped analyzer rule set.
- Produce the readiness evidence that A13 will enforce and publish through CI.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A11.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `docs/phase-A/production-readiness-checklist.md`

## Exact Targets

- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `examples/sc.lint.roslyn.demagic.package-smoke/`
- `eng/validate-sc-lint-roslyn-demagic-package.sh`
- `eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `docs/phase-A/production-readiness-checklist.md`
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/ProductionReadinessChecklistRow.cs`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`, `ProductionReadinessChecklistRow`

## Required Work

- make release metadata reflect the real implemented rule set and nothing more
- update repo documentation so analyzer readiness is described accurately and
  CLI work is not misrepresented as the primary Phase A outcome
- treat these files as authoritative gate artifacts for A12 closeout:
  `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`,
  `docs/phase-A/production-readiness-checklist.md`,
  `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`,
  `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`,
  `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- apply each gate artifact's own internal completion rule when judging closure;
  sprint wording may require the artifact, but does not override the artifact's
  own gate
- close the permutation matrix against its own publish-ready rule:
  every supported row must be `covered` or `unsupported`; no supported row may
  remain `planned`, implied, or otherwise open
- add a production-readiness checklist for:
  rule coverage, package consumption, metadata alignment, and documented
  validation commands
- close the production-readiness checklist against repository evidence:
  every required row must have passing evidence, named validation, and resolved
  status evidence; checklist presence alone is insufficient
- freeze the expected-diagnostics manifest and package-validation script outputs
  to the exact shippable analyzer rule set
- keep `AnalyzerReleases.Unshipped.md` empty except for the required header and
  ensure shipped release metadata, packaged-consumer diagnostics, and docs all
  describe the same rule set
- define `ProductionReadinessChecklistRow` so the checklist records:
  requirement area, artifact, validation command, owner, and status evidence
- treat A12 as incomplete until the gate artifacts are closed, not merely
  created or enumerated

## Acceptance Criteria

- docs no longer imply unimplemented analyzer behavior is production-ready
- rule metadata, release metadata, and sample corpus all describe the same
  shippable analyzer set
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` is closed under its own
  internal gate; every supported row is `covered` or `unsupported`
- `docs/phase-A/production-readiness-checklist.md` is complete and auditable,
  with no open required row
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md` contains no pending rule
  entries beyond the required header
- the packaged-consumer manifest and validation scripts describe the same
  shipped diagnostics that release metadata and docs describe
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json` matches the package
  validation assertions and packaged-consumer evidence
- no CI publication behavior is still ambiguous when A12 closes; A13 inherits a
  finished readiness contract rather than redefining one

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `bash eng/validate-sc-lint-roslyn-demagic-package.sh`
- `pwsh -File eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `git diff --check`
