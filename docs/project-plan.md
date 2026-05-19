# sc-lint-roslyn Suite Project Plan

## 1. Goal

Establish a formal documentation and delivery framework for the repository
while driving the first approved implementation line for `sc.lint.roslyn.demagic`.

The repository does not assume the current code is correct. If the existing
implementation conflicts with approved requirements or architecture, it may be
deleted and replaced.

## 2. Deliverables

Phase A deliverables:

- suite-level requirements, architecture, and project plan
- project-level requirements and architecture for `sc.lint.roslyn.demagic`
- project-level requirements and architecture for `sc-lint-roslyn`
- project-level boundary inventories for both projects
- accepted repository ADRs for enforceable Phase A decisions
- a sprinted Phase A plan
- a PRD-aligned `sc.lint.roslyn.demagic` v1 implementation
- a CLI design baseline aligned with the repository's AI-first CLI rules
- a production-ready analyzer validation path that covers every approved rule
- a packaged-consumer example proving the built analyzer works through a local
  feed
- CI gates that validate the packaged-consumer path before merge
- GitHub Packages publication for repo-produced packages, with the first
  NuGet.org release remaining manual and documented

## 3. Project Inventory

Current project inventory:

- `src/sc.lint.roslyn.demagic`
- `src/sc.lint.roslyn`
- `tests/sc.lint.roslyn.demagic.tests`
- `tests/sc.lint.roslyn.tests`

Owned project documents:

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn-demagic/boundaries.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/boundaries.md`
- `docs/documentation-guidelines.md`
- `docs/adr/INDEX.md`

## 4. Work Sequence

### Phase A: Formal Baseline and DeMagic v1

Phase A is the active planning line for this repository.

Execution branch:

- `integration/phase-A`

Merge target:

- `develop`

| Sprint | Scope | Required outcome |
| --- | --- | --- |
| A0 | Documentation reset | Replace placeholders and unapproved assumptions with formal suite and project docs |
| A1 | Analyzer foundation | Add reusable configuration and forbidden-pattern infrastructure for `sc.lint.roslyn.demagic` |
| A2 | `DM002` forbidden-pattern analyzer | Align forbidden-string analysis, config parsing, and analyzer validation with the PRD |
| A3 | `DM002` hardening and release alignment | Remove remaining spike leftovers, align release metadata, and route analyzer seams through interfaces |
| A4 | Packaging and CLI baseline correction | Finalize analyzer packaging gates and lock the CLI design baseline to AI-first contract rules |
| A5 | CLI foundation and abstractions package | Replace the Spectre spike with the first working `System.CommandLine` host and shared tool-module abstractions |
| A6 | DeMagic backend integration and first usable lint flow | Deliver `sc-lint-roslyn lint demagic` and the first usable `lint fast` smoke path |
| A7 | Profiles plus check, clippy, and ci workflows | Deliver reusable lint profiles and the first .NET-native `check`, `clippy`, and `ci` workflows |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Harden the multi-tool CLI surface and delegated backend seams |
| A9 | `DM001` completion and rule parity | Implement the missing constant-consolidation analyzer behavior and close the rule gap |
| A10 | Analyzer sample corpus and rule matrix | Add exhaustive analyzer samples and traceability for every rule and corner case |
| A11 | Packaged consumer validation | Pack the analyzer and consume it from a normal project via a local feed |
| A12 | Production-readiness convergence | Align package metadata, docs, sample validation, and readiness evidence to the shippable analyzer set |
| A13 | CI publish and manual release handoff | Add CI package-consumer gates, GitHub Packages publication, and the documented manual NuGet.org first release path |

Phase A must not treat the current CLI spike as an approved product contract.

Phase A continuation after the initial CLI-baseline and analyzer-foundation
sprints is:

- `A9` closes the missing `DM001` implementation so the analyzer actually
  matches the approved rule inventory.
- `A10` adds the exhaustive analyzer sample corpus and requirement traceability
  matrix.
- `A11` validates the built analyzer as a locally packed package consumed from
  a normal project.
- `A12` converges docs, manifests, release metadata, and readiness evidence
  onto one real shippable analyzer set.
- `A13` adds CI enforcement for packaged-consumer validation and staged
  publication, while keeping the first NuGet.org release manual.

### Phase A Implementation Inventory

Phase A implementation work is expected to touch, add, delete, or replace
these code paths:

- `src/sc.lint.roslyn.demagic/analyzers/MagicNumberAnalyzer.cs` deleted in A3
- `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `examples/sc.lint.roslyn.demagic.package-smoke/`
- `eng/validate-sc-lint-roslyn-demagic-package.sh`
- `eng/validate-sc-lint-roslyn-demagic-package.ps1`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/`
- `docs/phase-A/production-readiness-checklist.md`
- `docs/releasing.md`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM002ForbiddenStringLiteralAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/analyzers/DM001ConstantConsolidationAnalyzerTests.cs`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm001/`
- `tests/sc.lint.roslyn.demagic.tests/testdata/dm002/`
- `tests/sc.lint.roslyn.demagic.tests/testdata/README.md`
- `tests/sc.lint.roslyn.demagic.tests/PermutationMatrix.md`
- `src/sc.lint.roslyn/Program.cs`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

Delivered Phase A analyzer implementation units:

- `src/sc.lint.roslyn.demagic/configuration/` for analyzer config selection, parsing,
  and immutable `DM001`/`DM002` option models
- `src/sc.lint.roslyn.demagic/diagnostics/` for centralized diagnostic descriptor
  ownership
- `src/sc.lint.roslyn.demagic/patterns/` for forbidden-pattern compilation and match
  behavior
- `src/sc.lint.roslyn.demagic/analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`

Delivered Phase A CLI and tooling baseline units:

- `src/sc.lint.roslyn.abstractions/` for shared tool descriptors, command
  handlers, workspace seam, and stable envelope contracts
- `src/sc.lint.roslyn/commands/` for `System.CommandLine` registration of the
  shipped Phase A command families
- `src/sc.lint.roslyn/commandmodel/` and `src/sc.lint.roslyn/contracts/` for the
  stable CLI contract surface and workflow result models
- `src/sc.lint.roslyn/dispatch/` for in-process and delegated backend
  normalization and process seams
- `src/sc.lint.roslyn/operations/` for reusable command operations and workflow
  runners
- `src/sc.lint.roslyn/serialization/` and `src/sc.lint.roslyn/formatting/` for JSON
  envelope serialization and human output
- `tests/sc.lint.roslyn.tests/contracts/` and
  `tests/sc.lint.roslyn.tests/operations/` for contract and workflow coverage

Delivered Phase A package-validation support units:

- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/ExpectedPackageDiagnostic.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationManifest.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationResult.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/PackageValidationSampleKind.cs`
- `tests/sc.lint.roslyn.demagic.tests/packagevalidation/ProductionReadinessChecklistRow.cs`

## 5. Execution Rules

- predecessor sprint QA closure blocks sprint closeout, not sprint start
- later sprint branches may begin implementation before earlier sprint QA is
  complete
- before declaring a later sprint complete, merge forward the latest accepted
  earlier-sprint state and rerun required validation on the resulting head
- requirements and architecture documents are authoritative over the current
  spike implementation
- analyzer-first work takes precedence over speculative CLI feature work until
  the analyzer baseline is approved
- after A8, Phase A work returns to analyzer production-readiness and package
  consumer validation before any further CLI scope is considered
- GitHub Packages publication work remains analyzer-first support work and must
  not displace unfinished analyzer rule, sample, or package-consumer gaps
- future CLI implementation must inherit the contract rules defined in
  `docs/sc-lint-roslyn/requirements.md` and `docs/sc-lint-roslyn/architecture.md`
- if spike code does not comply with approved requirements or architecture, the
  preferred action is full removal and replacement rather than incremental
  editing to preserve unapproved structure
- implementation planning must use the project boundary documents to keep
  analyzer logic, CLI contract logic, and future adapter logic from collapsing
  into one project
- if a current file name or class name encodes the rejected spike semantics,
  replacement with new files and types is preferred over keeping the old names
  and editing their internals

## 6. Phase A Acceptance

Phase A planning is complete only when:

- the repo documentation framework exists and is internally consistent
- `sc.lint.roslyn.demagic` behavior is planned against the PRD rather than against the
  current spike
- analyzer packaging and validation expectations are explicit in the sprint
  plans
- analyzer sample coverage and packaged-consumer validation are explicit in the
  sprint plans
- CI package-consumer validation and GitHub Packages publication expectations
  are explicit in the sprint plans
- the CLI baseline no longer treats the current implementation as an approved
  design
- the execution rules explicitly prefer deleting and replacing noncompliant
  spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets, named types, and validation
  commands to drive implementation directly

This acceptance block is the project-plan mirror of
`docs/phase-A/plan-phase-A.md` Section 8.
