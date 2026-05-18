---
id: A11
title: packaged consumer validation
status: complete
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

- `.github/workflows/ci.yml`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `examples/Directory.Build.props`
- `examples/Roslyn.DeMagic.PackageSmoke/NuGet.config`
- `examples/Roslyn.DeMagic.PackageSmoke/Directory.Build.props`
- `examples/Roslyn.DeMagic.PackageSmoke/Roslyn.DeMagic.PackageSmoke.csproj`
- `examples/Roslyn.DeMagic.PackageSmoke/.roslyn-lint/config-src.toml`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/PublicConstOutsideDesignatedFile.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/InternalConstOutsideDesignatedFile.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/DesignatedFileCompliantConst.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/SuppressedConst.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM001/UnsuppressedConstControl.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/ExactMatchConstField.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/PrefixMethodArgument.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/SuffixComparison.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/SubstringReturnValue.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/AttributeArgument.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/CompliantLiteral.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/SuppressedLiteral.cs`
- `examples/Roslyn.DeMagic.PackageSmoke/Samples/DM002/UnsuppressedLiteralControl.cs`
- `eng/validate-roslyn-demagic-package.sh`
- `eng/validate-roslyn-demagic-package.ps1`
- `eng/roslyn-demagic-package-expected-diagnostics.json`
- `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/ExpectedPackageDiagnostic.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationContractsTests.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationManifest.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationResult.cs`
- `tests/Roslyn.DeMagic.Tests/PackageValidation/PackageValidationSampleKind.cs`

## Important Interfaces, Records/Structs, And Enums

- immutable payload types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationResult`
- enums:
  `PackageValidationSampleKind`

## Required Work

- pack `Roslyn.DeMagic` from the repo build into a local package output
- configure a local NuGet feed for the example consumer project
- reference `Roslyn.DeMagic` by package reference, not by project reference
- keep `examples/` isolated from repo-root packaging metadata with a bare
  stop-file so the package-smoke subtree carries only its local overrides
- keep the package-smoke project on `.roslyn-lint/config-src.toml`; document
  that `.roslyn-lint/config-test.toml` routing remains validated by
  `DeMagicConfigLoaderTests` from A10 rather than by the A11 sample project
- add example source files that intentionally trigger every analyzer rule
- add compliant and suppression examples so the package consumer can verify both
  diagnostic presence and absence
- add `eng/roslyn-demagic-package-expected-diagnostics.json` so the validation
  scripts compare build output to a structured expected-diagnostics manifest
- add compiled validation-support types in
  `tests/Roslyn.DeMagic.Tests/PackageValidation/` so the manifest shape and
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

- the example project restores `Roslyn.DeMagic` from a locally built package
- the example project emits expected diagnostics for `DM001` and `DM002`
- compliant and suppressed samples behave as expected when the package is
  consumed normally
- the expected diagnostics are asserted from a structured manifest rather than
  by manual console inspection
- package-smoke validation closes with 9 expected diagnostics present and 4
  expected-clean files remaining clean
- `tests/Roslyn.DeMagic.Tests/PermutationMatrix.md` contains one row for every
  supported analyzer permutation and no implicit coverage gaps remain
- package-consumer validation does not rely on in-repo project references or
  special test-only analyzer wiring

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release -o artifacts/packages`
- `dotnet test tests/Roslyn.DeMagic.Tests/Roslyn.DeMagic.Tests.csproj --configuration Release --verbosity normal`
- `bash eng/validate-roslyn-demagic-package.sh`
- `git diff --check`
