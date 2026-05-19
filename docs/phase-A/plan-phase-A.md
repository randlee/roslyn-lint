# Phase A Plan

## 1. Goal

Phase A establishes the repository's formal documentation baseline and delivers
the first approved implementation line for `sc.lint.roslyn.demagic`.

The CLI is still part of the suite, but Phase A treats it as a contract-first
design line rather than assuming the current spike is valid.

## 2. Deliverables

- suite-level documentation framework
- project-level requirements and architecture for both products
- project-level boundary inventories for both products
- PRD-aligned `sc.lint.roslyn.demagic` design and implementation plan
- AI-first `sc-lint-roslyn` CLI baseline
- packaging and validation gates for the analyzer
- a production-ready `sc.lint.roslyn.demagic` analyzer package covering every approved
  rule
- a locally packaged analyzer-consumer example that proves the built package can
  be used from a normal .NET project
- CI gates that validate the packaged analyzer-consumer path on every relevant
  change
- GitHub Packages publication for repo-produced packages, with NuGet.org
  publication still documented as a manual first-release step

## 3. Execution Branch

- branch: `integration/phase-A`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/prd/sc-lint-roslyn-demagic-prd.md`
- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/sc-lint-roslyn-demagic/*`
- `docs/sc-lint-roslyn/*`
- `.claude/skills/creating-ai-clis/`

## 5. Exact Implementation Targets

- `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/configuration/`
- `src/sc.lint.roslyn.demagic/diagnostics/`
- `src/sc.lint.roslyn.demagic/patterns/`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/`
- `tests/sc.lint.roslyn.demagic.tests/testdata/README.md`
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- `examples/sc.lint.roslyn.demagic.package-smoke/`
- `eng/validate-sc-lint-roslyn-demagic-package.sh`
- `eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/`
- `src/sc.lint.roslyn/Program.cs`
- `src/sc.lint.roslyn.abstractions/contracts/`
- `src/sc.lint.roslyn/operations/`
- `src/sc.lint.roslyn/serialization/`
- `src/sc.lint.roslyn/formatting/`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `tests/sc.lint.roslyn.tests/commands/`
- `tests/sc.lint.roslyn.tests/contracts/`
- `tests/sc.lint.roslyn.tests/operations/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `docs/phase-A/production-readiness-checklist.md`
- `docs/releasing.md`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| A0 | Documentation reset | Replace placeholders and unapproved assumptions with approved suite and project docs |
| A1 | Analyzer foundation | Add reusable configuration and forbidden-pattern infrastructure for `sc.lint.roslyn.demagic` |
| A2 | `DM002` forbidden-pattern analyzer | Replace the generic string-literal spike with forbidden-pattern analysis and aligned release metadata |
| A3 | `DM002` hardening and release alignment | Remove remaining spike leftovers, align release metadata, and route analyzer seams through injected interfaces |
| A4 | Packaging and CLI baseline correction | Finalize analyzer package and release gates and define the replacement-oriented CLI baseline |
| A5 | CLI foundation and abstractions package | Replace the Spectre spike with the first working `System.CommandLine` host and shared tool-module abstractions |
| A6 | DeMagic backend integration and first usable lint flow | Deliver `sc-lint-roslyn lint demagic` and the first usable `lint fast` smoke path |
| A7 | Profiles plus check, clippy, and ci workflows | Deliver reusable lint profiles and the first .NET-native `check`, `clippy`, and `ci` workflows |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Harden the multi-tool CLI surface and delegated backend seams |
| A9 | `DM001` completion and rule parity | Implement the constant-consolidation rule against the PRD and close the analyzer parity gap |
| A10 | Analyzer sample corpus and rule matrix | Add exhaustive positive, negative, suppression, and corner-case samples for every analyzer rule |
| A11 | Packaged consumer validation | Prove the locally built `sc.lint.roslyn.demagic` package works from a normal consuming project via a local feed |
| A12 | Production-readiness convergence | Make analyzer metadata, sample consumption, docs, and readiness evidence reflect one real shippable rule set |
| A13 | CI publish and manual release handoff | Validate package-consumer gates in CI, publish repo packages to GitHub Packages, and document the manual NuGet.org first release |

## 7. Implementation Strategy

- predecessor sprint closure is a closeout gate, not a start gate
- later sprint development may begin before the previous sprint passes QA
- before a later sprint closes, it must merge forward the latest accepted
  earlier-sprint state and revalidate against it
- A0 documents the approved target and replacement policy
- A1 introduces shared configuration and forbidden-pattern infrastructure
- A2 deletes the `DM002` spike behavior and introduces compiled forbidden-
  pattern matching, config-driven severity, and analyzer metadata alignment
- A3 removes remaining `DM001` spike leftovers, keeps `DM001` metadata on the
  approved category, and hardens `DM002` through injected configuration and
  matcher seams
- A4 validates analyzer package outputs and leaves the CLI with a strict design
  baseline that future implementation must follow
- A5 through A8 establish the first usable CLI shell, but they do not replace
  the analyzer-first Phase A exit criteria
- A9 completes the missing `DM001` behavior so the analyzer package implements
  the full approved rule set
- A10 adds the required analyzer sample corpus and requirement-to-test matrix
  for every rule and documented corner case
- A11 proves package-consumer behavior by packing `sc.lint.roslyn.demagic` and
  consuming it from an example project through a local feed
- A12 converges release metadata, packaged-consumer evidence, and repo docs so
  the analyzer can enter production testing without relying on undocumented
  assumptions
- A13 adds the CI package-consumer gate, GitHub Packages publication path, and
  manual NuGet.org release handoff documentation
- no sprint in Phase A should preserve current spike semantics merely because
  code already exists
- no additional CLI expansion should take precedence over unfinished analyzer
  production-readiness work until A13 is complete

### 7.1 Named Type Inventory

Phase A planning now assumes these implementation types will exist or be
introduced during the development sprints:

- analyzer configuration and boundary types:
  `IAdditionalFileConfigSelector`, `IDeMagicConfigLoader`, `ITomlConfigParser`,
  `DeMagicConfig`, `Dm001Options`, `Dm002Options`,
  `AdditionalFileConfigSelection`, `ConfiguredSeverity`,
  `DeMagicConfigLoader`
- analyzer pattern types:
  `IForbiddenPatternCompiler`, `ForbiddenPattern`,
  `CompiledForbiddenPattern`, `ForbiddenPatternKind`,
  `ForbiddenPatternMatcher`
- sample-corpus and traceability types:
  `ExpectedDiagnostic`, `RequirementTraceabilityRow`, `AnalyzerSampleKind`
- CLI contract and boundary types:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendJsonNormalizer`,
  `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`, `ToolId`, `ToolDescriptor`,
  `CliEnvelope<TResult>`, `CliError`, `CliDiagnostic`, `CliErrorKind`,
  `BackendProcessRequest`, `BackendProcessResult`,
  `DelegatedBackendNormalizationResult<T>`, `LintToolRequest`,
  `LintToolResult`, `LintFinding`, `ViewRequest`, `ViewResult`,
  `ViewToolResult`, `ViewRuleResult`, `CheckRequest`, `CheckResult`,
  `ClippyRequest`, `ClippyResult`, `CiRequest`, `CiResult`,
  `VersionResult`
- package-validation types:
  `ExpectedPackageDiagnostic`, `PackageValidationManifest`,
  `PackageValidationSampleKind`, `PackageValidationResult`

The package-validation types above are planned compiled validation-support
types owned by `tests/sc.lint.roslyn.demagic.tests/packagevalidation/`. They are not
analyzer runtime types and must not live in `sc.lint.roslyn.demagic`.

The sample-corpus and traceability types above are planned compiled
test-support types owned by `tests/sc.lint.roslyn.demagic.tests/testing/`. They are
not analyzer runtime types and must not live in `sc.lint.roslyn.demagic`.

## 8. Acceptance

Phase A is complete only when:

- the repo documentation framework exists and is internally consistent
- `sc.lint.roslyn.demagic` behavior matches the PRD rather than the current spike
- analyzer packaging and tests reflect the approved diagnostic set
- analyzer sample coverage exists for every approved rule and documented corner
  case
- the locally built analyzer package is validated through a normal consuming
  project rather than only through in-repo unit tests
- CI validates the packaged-consumer path before Phase A is called complete
- GitHub Packages publication is configured for repo-produced packages, and the
  first NuGet.org release remains manual but documented
- the CLI baseline no longer treats the current implementation as an approved
  design
- the Phase A execution rules explicitly prefer deleting and replacing
  noncompliant spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets and validation commands to drive
  implementation directly
