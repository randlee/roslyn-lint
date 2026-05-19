---
id: A11
title: packaged consumer validation
status: complete
branch: sprint/A11
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A11
target: integration/phase-A
---

# Sprint A11 - Packaged Consumer Validation

## Goal

- Prove the built `sc.lint.roslyn.demagic` package works from a normal consuming
  project.
- Validate the analyzer as a package, not only as an in-repo project reference.
- Create the production-testing fixture that real users can run locally.

## Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/phase-A/sprint-A10.md`

## Exact Targets

- `.github/workflows/ci.yml`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `examples/Directory.Build.props`
- `examples/sc.lint.roslyn.demagic.package-smoke/NuGet.config`
- `examples/sc.lint.roslyn.demagic.package-smoke/Directory.Build.props`
- `examples/sc.lint.roslyn.demagic.package-smoke/sc.lint.roslyn.demagic.package-smoke.csproj`
- `examples/sc.lint.roslyn.demagic.package-smoke/.sc-lint-roslyn/config-src.toml`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm001/PublicConstOutsideDesignatedFile.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm001/InternalConstOutsideDesignatedFile.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm001/DesignatedFileCompliantConst.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm001/SuppressedConst.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm001/UnsuppressedConstControl.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/ExactMatchConstField.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/PrefixMethodArgument.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/SuffixComparison.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/SubstringReturnValue.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/AttributeArgument.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/CompliantLiteral.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/SuppressedLiteral.cs`
- `examples/sc.lint.roslyn.demagic.package-smoke/samples/dm002/UnsuppressedLiteralControl.cs`
- `eng/validate-sc-lint-roslyn-demagic-package.sh`
- `eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/ExpectedPackageDiagnostic.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationContractsTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationManifest.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationResult.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationSampleKind.cs`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`
- enums:
  `PackageValidationSampleKind`

## Required Work

- pack `sc.lint.roslyn.demagic` from the repo build into a local package output
- configure a local NuGet feed for the example consumer project
- reference `sc.lint.roslyn.demagic` by package reference, not by project reference
- keep `examples/` isolated from repo-root packaging metadata with a bare
  stop-file so the package-smoke subtree carries only its local overrides
- keep the package-smoke project on `.sc-lint-roslyn/config-src.toml`; document
  that `.sc-lint-roslyn/config-test.toml` routing remains validated by
  `DeMagicConfigLoaderTests` from A10 rather than by the A11 sample project
- add example source files that intentionally trigger every analyzer rule
- add compliant and suppression examples so the package consumer can verify both
  diagnostic presence and absence
- add `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json` so the validation
  scripts compare build output to a structured expected-diagnostics manifest
- add compiled validation-support types in
  `tests/sc.lint.roslyn.demagic.tests/packagevalidation/` so the manifest shape and
  expected diagnostic rows have one named project home
- complete a closed permutation matrix for every supported `DM001` and `DM002`
  analyzer combination before package-smoke signoff
- provide cross-platform validation scripts that pack, restore, build, and
  assert expected diagnostics from the consumer project
- wire CI to pack the analyzer into `artifacts/packages` and run the package
  validation scripts on both Unix and Windows runners
- define `PackageValidationSampleKind` so each example file is tagged as:
  positive, negative, suppression, or config-behavior

## Acceptance Criteria

- the example project restores `sc.lint.roslyn.demagic` from a locally built package
- the example project emits expected diagnostics for `DM001` and `DM002`
- compliant and suppressed samples behave as expected when the package is
  consumed normally
- the expected diagnostics are asserted from a structured manifest rather than
  by manual console inspection
- package-smoke validation closes with 9 expected diagnostics present and 4
  expected-clean files remaining clean
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md` contains one row for every
  supported analyzer permutation and no implicit coverage gaps remain
- package-consumer validation does not rely on in-repo project references or
  special test-only analyzer wiring

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release -o artifacts/packages`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `bash eng/validate-sc-lint-roslyn-demagic-package.sh`
- `git diff --check`
