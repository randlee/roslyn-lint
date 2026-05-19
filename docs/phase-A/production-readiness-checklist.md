# Phase A Production-Readiness Checklist

This checklist is the authoritative A12 signoff artifact for `sc.lint.roslyn.demagic`
Phase A v0.1.2 production testing.

All rows must be complete and backed by concrete evidence before A12 closes.

| Requirement area | Artifact | Validation command | Owner | Status evidence |
| --- | --- | --- | --- | --- |
| Shipped rule inventory | `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md` | inspect shipped rule table; `dotnet build sc-lint-roslyn.sln --configuration Release` | `crl` | `2026-05-18`: `DM001` and `DM002` are the only Phase A v0.1.2 shipped rules and both entries include the release date |
| Unshipped rule inventory | `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md` | inspect file contents during A12 signoff | `crl` | `2026-05-18`: no stray unshipped Phase A rule entries remain |
| Requirement traceability | `tests/sc.lint.roslyn.demagic.tests/TestMatrix.md` | `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal` | `crl` | `2026-05-18`: every approved `REQ-DM*` row maps to concrete analyzer samples and automated tests |
| Supported permutation closure | `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` | review supported rows during A12 signoff | `crl` | `2026-05-18`: the matrix contains zero `planned` rows; every listed permutation is `covered` or `unsupported`, and the package-smoke subset matches real sample files |
| Packaged-consumer manifest | `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json` | `bash eng/validate-sc-lint-roslyn-demagic-package.sh`; `pwsh -File eng/validate-sc-lint-roslyn-demagic-package.ps1` | `crl` | `2026-05-18`: the manifest matches the shipped Phase A v0.1.2 analyzer set with `9` expected diagnostics and `4` expected-clean files |
| Packaged-consumer sample project | `examples/sc.lint.roslyn.demagic.package-smoke/` | `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`; `bash eng/validate-sc-lint-roslyn-demagic-package.sh` | `crl` | `2026-05-18`: the locally packed analyzer restores and reports expected diagnostics through a normal consumer project |
| Package metadata alignment | `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj` | `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages` | `crl` | `2026-05-18`: package description, tags, readme, and release notes all describe the same shipped analyzer set |
| Repo documentation alignment | `README.md`; `docs/sc-lint-roslyn-demagic/architecture.md` | A12 doc review + `git diff --check` | `crl` | `2026-05-18`: docs describe analyzer readiness accurately and do not represent unfinished CLI work as the primary Phase A outcome |
| Release-note readiness | `docs/releasing.md` | A12 doc review + `git diff --check` | `crl` | `2026-05-18`: Phase A v0.1.2 release notes and release handoff expectations are documented for maintainer review |
