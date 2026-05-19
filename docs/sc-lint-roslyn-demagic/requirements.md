# sc.lint.roslyn.demagic Requirements

## 1. Product Definition

`sc.lint.roslyn.demagic` is the first analyzer package in the `sc-lint-roslyn` suite.

This document turns the PRD in `docs/prd/sc-lint-roslyn-demagic-prd.md` into the
approved requirements baseline for implementation and review.

Product requirement IDs:

- `REQ-DM-PRODUCT-001` `sc.lint.roslyn.demagic` must ship as a Roslyn analyzer package
  consumable by standard .NET projects.
- `REQ-DM-PRODUCT-002` Version 1 consists of exactly two configurable
  diagnostics: `DM001` and `DM002`.
- `REQ-DM-PRODUCT-003` The analyzer must have no runtime dependency on the
  `sc-lint-roslyn` CLI executable.

## 2. Configuration Model

Configuration requirement IDs:

- `REQ-DM-CONFIG-001` Analyzer behavior must be driven by files under
  `.sc-lint-roslyn/` at the solution root.
- `REQ-DM-CONFIG-002` Source projects use `.sc-lint-roslyn/config-src.toml`.
- `REQ-DM-CONFIG-003` Test projects use `.sc-lint-roslyn/config-test.toml`.
- `REQ-DM-CONFIG-004` The analyzer must receive configuration through
  `AdditionalFiles` wiring in `Directory.Build.props`.
- `REQ-DM-CONFIG-005` If no applicable config file is present, the analyzer
  must disable its rules gracefully rather than fail the compilation.
- `REQ-DM-CONFIG-006` Rule severity must be configurable per rule.
- `REQ-DM-CONFIG-007` Configuration selection must be driven by the
  `AdditionalFiles` payload passed to the analyzer, not by filename probing in
  the source tree during analysis.
- `REQ-DM-CONFIG-008` Invalid configuration must not crash analysis; the
  affected rule must fail closed and remain non-reporting until valid config is
  supplied.

## 3. DM001 Requirements

Rule requirement IDs:

- `REQ-DM001-001` `DM001` diagnoses public or internal `const` field
  declarations that are outside the designated constants file.
- `REQ-DM001-002` `DM001` uses the configured `designated_file` as the primary
  location rule.
- `REQ-DM001-003` If `designated_class` is configured, `DM001` must also
  require the containing type to match.
- `REQ-DM001-004` `DM001` must not diagnose private or protected constants.
- `REQ-DM001-005` `DM001` must not diagnose local constants inside method
  bodies.
- `REQ-DM001-006` `DM001` must remain suppressible through normal Roslyn
  suppression mechanisms.

## 4. DM002 Requirements

Rule requirement IDs:

- `REQ-DM002-001` `DM002` diagnoses string literals whose values match a
  configured forbidden pattern.
- `REQ-DM002-002` Supported pattern forms are exact, prefix, suffix, and
  substring matching.
- `REQ-DM002-003` Matching is case-insensitive by default and configurable with
  `case_sensitive`.
- `REQ-DM002-004` `DM002` applies to string literals regardless of expression
  context, including method arguments, comparisons, return values, switch arms,
  and attribute arguments when those appear as literal syntax.
- `REQ-DM002-005` `DM002` does not inspect comments, XML documentation, or
  interpolated string holes in v1.
- `REQ-DM002-006` `DM002` must remain suppressible through normal Roslyn
  suppression mechanisms.

## 5. Diagnostics Behavior

Diagnostic requirement IDs:

- `REQ-DM-DIAG-001` Diagnostic IDs are `DM001` and `DM002`.
- `REQ-DM-DIAG-002` Diagnostic messages must explain the violation in terms of
  constant consolidation or forbidden domain coupling, not generic "magic
  value" wording.
- `REQ-DM-DIAG-003` The analyzer must honor configured severities rather than
  hard-coding effective severity in rule behavior.
- `REQ-DM-DIAG-004` Code fixes are out of scope for v1.
- `REQ-DM-DIAG-005` Diagnostic categories must reflect the approved product
  semantics rather than the current spike categories:
  `sc.lint.roslyn.organization` for `DM001` and `sc.lint.roslyn.domainboundary` for
  `DM002`.

## 6. Packaging Requirements

Packaging requirement IDs:

- `REQ-DM-PACK-001` The package ID is `sc.lint.roslyn.demagic`.
- `REQ-DM-PACK-002` The analyzer assembly targets `netstandard2.0`.
- `REQ-DM-PACK-003` The package must ship as an analyzer package with no
  application runtime dependency.
- `REQ-DM-PACK-004` Package metadata and release automation must reflect the
  analyzer's actual rule behavior rather than the current spike description.
- `REQ-DM-PACK-005` The package description, tags, and README references must
  describe constant-consolidation and forbidden-string diagnostics, not generic
  magic-number or magic-string linting.
- `REQ-DM-PACK-006` CI must be able to publish `sc.lint.roslyn.demagic` and any
  repo-produced package dependencies to GitHub Packages.
- `REQ-DM-PACK-007` The first NuGet.org publication remains manual; the manual
  release handoff must be documented before automation is considered.

## 7. Validation Requirements

Validation requirement IDs:

- `REQ-DM-TEST-001` Tests must cover positive and negative cases for both
  `DM001` and `DM002`.
- `REQ-DM-TEST-002` Tests must validate configuration-driven severity and
  enablement behavior.
- `REQ-DM-TEST-003` Tests must validate missing-config graceful disablement.
- `REQ-DM-TEST-004` Analyzer release metadata must stay aligned with the
  shipped diagnostic set.
- `REQ-DM-TEST-005` Tests for `DM001` and `DM002` must be rewritten if needed;
  preserving spike-era test names or expectations is not a requirement.
- `REQ-DM-TEST-006` The repository must contain positive, negative,
  suppression, and corner-case samples for every approved analyzer rule.
- `REQ-DM-TEST-007` The repository must validate the packaged
  `sc.lint.roslyn.demagic` analyzer through a normal consuming project that references
  a locally built NuGet package from a local feed.
- `REQ-DM-TEST-008` The analyzer test matrix must map every PRD rule checkbox
  to one or more concrete automated tests or packaged-consumer samples.
- `REQ-DM-TEST-009` CI must run the packaged-consumer validation path rather
  than relying only on in-repo analyzer tests.
- `REQ-DM-TEST-010` The packaged-consumer validation path must assert expected
  diagnostics from a structured manifest rather than by manual console review.
