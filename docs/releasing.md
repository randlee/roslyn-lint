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
