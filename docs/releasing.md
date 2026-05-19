# Releasing

## Phase A v0.1.0 Release Notes

Phase A v0.1.0 is the first production-testing release candidate for
`Roslyn.DeMagic`.

Delivered analyzer scope:

- `DM001` constant consolidation for public and internal constants
- `DM002` forbidden string literal detection driven by `.roslyn-lint`
  configuration

Delivered validation scope:

- analyzer requirement traceability in
  `tests/Roslyn.DeMagic.Tests/TestMatrix.md`
- supported-permutation closure in
  `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md`
- packaged-consumer validation through
  `examples/Roslyn.DeMagic.PackageSmoke/`
- machine-readable package-smoke expectations in
  `eng/roslyn-demagic-package-expected-diagnostics.json`

Phase A v0.1.0 does not include:

- code fixes for `DM001` or `DM002`
- expanded rule inventory beyond `DM001` and `DM002`
- automated NuGet.org publication

## Phase A Release Handoff

A12 closes the analyzer production-readiness contract. A13 completes the CI
publish and release handoff work.

Handoff expectations:

- GitHub Actions must continue to validate the packaged-consumer path before
  publication behavior is considered passing
- `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md` and
  `docs/phase-A/production-readiness-checklist.md` remain human-audit gate
  artifacts: they must be reviewed as closed on the branch under their own
  internal rules before release, but they are not treated as markdown grep
  checks in CI
- GitHub Packages publication is the automated package-distribution path for
  Phase A
- the first NuGet.org release remains manual and must not be driven directly by
  CI
- maintainers should treat
  `docs/phase-A/production-readiness-checklist.md` as the release gate input

## Maintainer Review Before A13

Before the first Phase A release is published anywhere, maintainers should
confirm:

- `AnalyzerReleases.Shipped.md` and `AnalyzerReleases.Unshipped.md` reflect the
  same rule set described in docs
- package-smoke validation passes from the locally packed analyzer package
- the expected-diagnostics manifest still matches the shipped analyzer set
- GitHub Packages publication behavior and manual NuGet.org steps are documented
  consistently

## GitHub Packages Publication Contract

Phase A automated publication is limited to GitHub Packages.

Published artifacts:

- `Roslyn.DeMagic.<version>.nupkg`
- `roslyn-lint.<version>.nupkg`

Publication expectations:

- the publish workflow packs the same analyzer and tool artifacts used by local
  validation
- publication happens only after restore, release build, solution tests, and
  packaged-consumer validation pass
- package-source credentials come from CI secrets and workflow configuration,
  not from sample-project local setup

## Manual NuGet.org First-Release Procedure

The first NuGet.org release remains manual for both `Roslyn.DeMagic` and the
`roslyn-lint` .NET tool package.

### Prerequisites

1. Confirm NuGet.org ownership for the `Roslyn.DeMagic` and `roslyn-lint`
   package identifiers.
2. Create or rotate a scoped NuGet.org API key with push rights for those
   packages.
3. Confirm the current signing policy:
   Phase A packages are not signed by an automated signing pipeline. If the
   release policy changes to require signed artifacts, stop and add signing
   before publish rather than improvising at release time.
4. Ensure a clean checkout of the release tag and authenticated access to the
   required package feeds.

### Build And Validate

1. `dotnet restore roslyn-lint.sln`
2. `dotnet build roslyn-lint.sln --configuration Release`
3. `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
4. `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
5. `dotnet pack src/Roslyn.Lint/Roslyn.Lint.csproj --configuration Release -o artifacts/packages`
6. `bash eng/validate-roslyn-demagic-package.sh`
7. `pwsh -File eng/validate-roslyn-demagic-package.ps1`

### Push Commands

Use a NuGet.org API key exported in `NUGET_API_KEY`:

```bash
dotnet nuget push artifacts/packages/Roslyn.DeMagic.0.1.0.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" \
  --skip-duplicate

dotnet nuget push artifacts/packages/roslyn-lint.0.1.0.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" \
  --skip-duplicate
```

### Post-Release Verification

Success criteria:

- NuGet.org lists both packages at the released version
- the analyzer package installs into a clean sample project and still reports
  the expected diagnostics
- the tool package installs as a .NET tool and exposes `roslyn-lint version`

Verification checklist:

1. `dotnet new classlib -n RoslynDeMagicNugetSmoke`
2. add `Roslyn.DeMagic` from NuGet.org and build against a known sample file
3. `dotnet tool install --global roslyn-lint --version 0.1.0`
4. run `roslyn-lint version --json`
5. compare NuGet.org package metadata against `README.md` and the shipped
   release notes

## Automation Boundary After First Release

Only after the first manual NuGet.org publication is completed and verified may
the repository consider automating NuGet.org pushes in a later sprint or ADR.
