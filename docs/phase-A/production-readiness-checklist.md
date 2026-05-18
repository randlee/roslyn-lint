# Phase A Production-Readiness Checklist

This checklist is the authoritative A12 signoff artifact for `Roslyn.DeMagic`
Phase A v0.1.0 production testing.

All rows must be complete and backed by concrete evidence before A12 closes.

| Requirement area | Artifact | Validation command | Owner | Status evidence |
| --- | --- | --- | --- | --- |
| Shipped rule inventory | `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md` | inspect shipped rule table; `dotnet build roslyn-lint.sln --configuration Release` | `crl` | `DM001` and `DM002` are the only Phase A v0.1.0 shipped rules and both entries include the release date |
| Unshipped rule inventory | `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md` | inspect file contents during A12 signoff | `crl` | no stray unshipped Phase A rule entries remain |
| Requirement traceability | `tests/Roslyn.DeMagic.Tests/TestMatrix.md` | `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal` | `crl` | every approved `REQ-DM*` row maps to concrete analyzer samples and automated tests |
| Supported permutation closure | `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md` | review supported rows during A12 signoff | `crl` | supported analyzer permutations are enumerated explicitly and packaged-consumer rows match real sample files |
| Packaged-consumer manifest | `eng/roslyn-demagic-package-expected-diagnostics.json` | `bash eng/validate-roslyn-demagic-package.sh`; `pwsh -File eng/validate-roslyn-demagic-package.ps1` | `crl` | manifest matches the shipped Phase A v0.1.0 analyzer set with `9` expected diagnostics and `4` expected-clean files |
| Packaged-consumer sample project | `examples/Roslyn.DeMagic.PackageSmoke/` | `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`; `bash eng/validate-roslyn-demagic-package.sh` | `crl` | locally packed analyzer restores and reports expected diagnostics through a normal consumer project |
| Package metadata alignment | `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj` | `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages` | `crl` | package description, tags, readme, and release notes all describe the same shipped analyzer set |
| Repo documentation alignment | `README.md`; `docs/roslyn-demagic/architecture.md` | A12 doc review + `git diff --check` | `crl` | docs describe analyzer readiness accurately and do not represent unfinished CLI work as the primary Phase A outcome |
| Release-note readiness | `docs/releasing.md` | A12 doc review + `git diff --check` | `crl` | Phase A v0.1.0 release notes and release handoff expectations are documented for maintainer review |
