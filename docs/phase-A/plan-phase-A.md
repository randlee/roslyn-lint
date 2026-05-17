# Phase A Plan

## 1. Goal

Phase A establishes the repository's formal documentation baseline and delivers
the first approved implementation line for `Roslyn.DeMagic`.

The CLI is still part of the suite, but Phase A treats it as a contract-first
design line rather than assuming the current spike is valid.

## 2. Deliverables

- suite-level documentation framework
- project-level requirements and architecture for both products
- a dedicated CLI contract document for `roslyn-lint`
- project-level boundary inventories for both products
- PRD-aligned `Roslyn.DeMagic` design and implementation plan
- AI-first `roslyn-lint` CLI baseline
- packaging and validation gates for the analyzer
- a usable `roslyn-lint` executable with the first real tool integrations

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
- `src/Roslyn.Lint.Abstractions/`
- `src/Roslyn.Lint/Commands/`
- `src/Roslyn.Lint/Contracts/`
- `src/Roslyn.Lint/Dispatch/`
- `src/Roslyn.Lint/Operations/`
- `src/Roslyn.Lint/Serialization/`
- `src/Roslyn.Lint/Formatting/`
- `src/Roslyn.Lint/Backends/`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/`
- `tests/Roslyn.Lint.Tests/Abstractions/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Dispatch/`
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
| A4 | CLI orchestration planning reset | Replace the ad hoc CLI baseline with a `creating-ai-clis` and `sc-lint`-aligned orchestration plan |
| A5 | CLI foundation and abstractions package | Replace Spectre with `System.CommandLine`, add `Roslyn.Lint.Abstractions`, and ship working `version` and `view tools` commands |
| A6 | DeMagic backend integration and first usable lint flow | Implement `lint roslyn-demagic` through the shared dispatch and contract seams |
| A7 | Profiles plus check, clippy, and ci workflows | Implement real local workflow commands using `.NET`-native tooling |
| A8 | View surfaces, boundary metadata, and tool-module hardening | Implement stable `view` targets and harden the multi-tool module model |

## 7. Implementation Strategy

- A0 documents the approved target and replacement policy
- A1 introduces shared configuration and forbidden-pattern infrastructure
- A2 deletes the `DM002` spike behavior and introduces compiled forbidden-
  pattern matching, config-driven severity, and analyzer metadata alignment
- A3 removes remaining `DM001` spike leftovers, keeps `DM001` metadata on the
  approved category, and hardens `DM002` through injected configuration and
  matcher seams
- A4 leaves the CLI with a strict orchestration and contract baseline that
  future implementation must follow before any new CLI code lands
- A5 establishes the real CLI host, abstractions package, and first always-on
  utility commands
- A6 makes `roslyn-demagic` usable through the top-level CLI
- A7 turns command families into real local developer workflows
- A8 completes the first usable multi-tool surface and boundary metadata model
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
  `ToolId`, `ToolDescriptor`, `ILintToolModule`,
  `ILintToolCommandHandler<TRequest, TResponse>`,
  `CommandFamily`, `LintProfile`, `OutputMode`, `BackendExecutionMode`,
  `IBackendToolDispatcher`, `IBackendProcessRunner`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`, `CliEnvelope<TResult>`, `CliError`,
  `CliDiagnostic`, `LintToolRequest`, `LintToolResult`, `LintFinding`,
  `ViewRequest`, `ViewResult`, `CheckRequest`, `CheckResult`,
  `ClippyRequest`, `ClippyResult`, `CiRequest`, `CiResult`, `VersionResult`,
  `CliErrorKind`

## 8. Acceptance

Phase A is complete only when:

- the repo documentation framework exists and is internally consistent
- `Roslyn.DeMagic` behavior matches the PRD rather than the current spike
- analyzer packaging and tests reflect the approved diagnostic set
- the CLI baseline no longer treats the current implementation as an approved
  design
- the CLI baseline explicitly defines the `roslyn-lint` command families,
  command identifiers, top-level envelope, parser choice, and backend-dispatch
  seams
- the CLI implementation exists and supports the first usable commands:
  `version`, `view tools`, `lint roslyn-demagic`, `lint fast`, `lint full`,
  `lint ci`, `check`, `clippy`, and top-level `ci`
- the Phase A execution rules explicitly prefer deleting and replacing
  noncompliant spike code over preserving it through compatibility-driven edits
- sprint plans contain enough exact targets and validation commands to drive
  implementation directly
