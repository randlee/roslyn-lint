# Phase A Plan

## 1. Goal

Phase A establishes the repository's formal documentation baseline and delivers
the first approved implementation line for `Roslyn.DeMagic`.

The CLI is still part of the suite, but Phase A treats it as a contract-first
design line rather than assuming the current spike is valid.

## 2. Deliverables

- suite-level documentation framework
- project-level requirements and architecture for both products
- project-level boundary inventories for both products
- PRD-aligned `Roslyn.DeMagic` design and implementation plan
- AI-first `roslyn-lint` CLI foundation
- packaging and validation gates for the analyzer

## 3. Execution Branch

- branch: `integration/phase-A`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/prd/roslyn-demagic-prd.md`
- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/roslyn-demagic/*`
- `docs/roslyn-lint/*`
- `.claude/skills/creating-ai-clis/`

## 5. Exact Implementation Targets

- `src/Roslyn.DeMagic/Analyzers/DM001ConstantConsolidationAnalyzer.cs`
- `src/Roslyn.DeMagic/Analyzers/DM002ForbiddenStringLiteralAnalyzer.cs`
- `src/Roslyn.DeMagic/Configuration/`
- `src/Roslyn.DeMagic/Diagnostics/`
- `src/Roslyn.DeMagic/Patterns/`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `tests/Roslyn.DeMagic.Tests/Analyzers/`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Contracts/`
- `src/Roslyn.Lint/Operations/`
- `src/Roslyn.Lint/Serialization/`
- `src/Roslyn.Lint/Formatting/`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Operations/`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| A0 | Documentation reset | Replace placeholders and unapproved assumptions with approved suite and project docs |
| A1 | Analyzer foundation | Add reusable configuration and forbidden-pattern infrastructure for `Roslyn.DeMagic` |
| A2 | `DM002` forbidden-pattern analyzer | Replace the generic string-literal spike with forbidden-pattern analysis and aligned release metadata |
| A3 | `DM002` hardening and release alignment | Remove remaining spike leftovers, align release metadata, and route analyzer seams through injected interfaces |
| A4 | Packaging and CLI baseline correction | Finalize analyzer package and release gates and define the replacement-oriented CLI baseline |
| A5 | CLI foundation and abstractions package | Replace the Spectre spike with the first working System.CommandLine host and shared tool-module abstractions |
| A6 | DeMagic backend integration and first usable lint flow | Deliver `roslyn-lint lint demagic` and the first usable `lint fast` smoke path |
| A7 | Profiles plus check, clippy, and ci workflows | Deliver reusable lint profiles and the first .NET-native `check`, `clippy`, and `ci` workflows |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Harden the multi-tool CLI surface and delegated backend seams |

## 7. Implementation Strategy

- A0 documents the approved target and replacement policy
- A1 introduces shared configuration and forbidden-pattern infrastructure
- A2 deletes the `DM002` spike behavior and introduces compiled forbidden-
  pattern matching, config-driven severity, and analyzer metadata alignment
- A3 removes remaining `DM001` spike leftovers, keeps `DM001` metadata on the
  approved category, and hardens `DM002` through injected configuration and
  matcher seams
- A4 validates analyzer package outputs and leaves the CLI with a strict design
  baseline that future implementation must follow
- A5 replaces the disposable Spectre host with the first working
  `System.CommandLine` implementation, shared abstractions, and stable JSON
  envelopes for `version` and `view tools`
- A6 introduces the first real lint backend path and the initial `lint fast`
  smoke-test profile
- A7 adds reusable lint profiles plus `.NET`-native `check`, `clippy`, and
  top-level `ci` workflows
- A8 hardens the view/reporting surface and delegated backend seams for
  additional suite tools
- no sprint in Phase A should preserve current spike semantics merely because
  code already exists

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
- CLI contract and boundary types:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`, `ToolId`, `ToolDescriptor`,
  `CliEnvelope<TResult>`, `CliError`, `CliDiagnostic`, `CliErrorKind`,
  `LintToolRequest`, `LintToolResult`, `LintFinding`, `ViewRequest`,
  `ViewResult`, `CheckRequest`, `CheckResult`, `ClippyRequest`,
  `ClippyResult`, `CiRequest`, `CiResult`, `VersionResult`

## 8. Acceptance

Phase A is complete only when:

- the repo documentation framework exists and is internally consistent
- `Roslyn.DeMagic` behavior matches the PRD rather than the current spike
- analyzer packaging and tests reflect the approved diagnostic set
- the CLI baseline no longer treats the current implementation as an approved
  design
- the Phase A execution rules explicitly prefer deleting and replacing
  noncompliant spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets and validation commands to drive
  implementation directly
