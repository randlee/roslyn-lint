# Releasing

## Phase A v0.1.1 Release Notes

Phase A v0.1.1 is the current production-testing release candidate for
`sc.lint.roslyn.demagic`.

Delivered analyzer scope:

- `DM001` constant consolidation for public and internal constants
- `DM002` forbidden string literal detection driven by `.sc-lint-roslyn`
  configuration

Delivered validation scope:

- analyzer requirement traceability in
  `tests/sc.lint.roslyn.demagic.tests/TestMatrix.md`
- supported-permutation closure in
  `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- packaged-consumer validation through
  `examples/sc.lint.roslyn.demagic.package-smoke/`
- machine-readable package-smoke expectations in
  `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`

Phase A v0.1.1 does not include:

- code fixes for `DM001` or `DM002`
- expanded rule inventory beyond `DM001` and `DM002`
- automated NuGet.org publication

## Phase A Release Handoff

A12 closes the analyzer production-readiness contract. A13 completes the CI
publish and release handoff work.

Handoff expectations:

- GitHub Actions must continue to validate the packaged-consumer path before
  publication behavior is considered passing
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` and
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

## Maintainer Review Before First Release

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

- `sc.lint.roslyn.demagic.<version>.nupkg`
- `sc-lint-roslyn.<version>.nupkg`

Publication expectations:

- the publish workflow packs the same analyzer and tool artifacts used by local
  validation
- publication happens only after restore, release build, solution tests, and
  packaged-consumer validation pass
- package-source credentials come from CI secrets and workflow configuration,
  not from sample-project local setup

## Manual NuGet.org First-Release Procedure

The first NuGet.org release remains manual for both `sc.lint.roslyn.demagic` and the
`sc-lint-roslyn` .NET tool package.

### Prerequisites

1. Confirm NuGet.org ownership for the `sc.lint.roslyn.demagic` and `sc-lint-roslyn`
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

1. `dotnet restore sc-lint-roslyn.sln`
2. `dotnet build sc-lint-roslyn.sln --configuration Release`
3. `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
4. `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
5. `dotnet pack src/sc.lint.roslyn/sc.lint.roslyn.csproj --configuration Release -o artifacts/packages`
6. `bash eng/validate-sc-lint-roslyn-demagic-package.sh`
7. `pwsh -File eng/validate-sc-lint-roslyn-demagic-package.ps1`

### Push Commands

Use a NuGet.org API key exported in `NUGET_API_KEY`:

```bash
dotnet nuget push artifacts/packages/sc-lint-roslyn-demagic.0.1.1.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" \
  --skip-duplicate

dotnet nuget push artifacts/packages/sc-lint-roslyn.0.1.1.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" \
  --skip-duplicate
```

### Post-Release Verification

Success criteria:

- NuGet.org lists both packages at the released version
- the analyzer package installs into a clean sample project and still reports
  the expected diagnostics
- the tool package installs as a .NET tool and exposes `sc-lint-roslyn version`

Verification checklist:

1. `dotnet new classlib -n ScLintRoslynDeMagicNugetSmoke`
2. add `sc.lint.roslyn.demagic` from NuGet.org and build against a known sample file
3. `dotnet tool install --global sc-lint-roslyn --version 0.1.1`
4. run `sc-lint-roslyn version --json`
5. compare NuGet.org package metadata against `README.md` and the shipped
   release notes

## Automation Boundary After First Release

Only after the first manual NuGet.org publication is completed and verified may
the repository consider automating NuGet.org pushes in a later sprint or ADR.
