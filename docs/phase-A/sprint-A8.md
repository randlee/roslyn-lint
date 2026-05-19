---
id: A8
title: View surfaces, boundary metadata, and tool-module hardening
status: complete
branch: sprint/A8
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A8
target: integration/phase-A
---

# Sprint A8 - View Surfaces, Boundary Metadata, And Tool-Module Hardening

## Goal

- Complete the first usable multi-tool foundation for `sc-lint-roslyn`.
- Implement the first stable `view` targets.
- Put the abstractions package to work for suite-specific boundary and tool
  metadata.

## Hard Dependencies

- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/phase-A/sprint-A7.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-005-sc-lint-roslyn-abstractions-package.md`

## Exact Targets

- `src/sc.lint.roslyn.abstractions/contracts/ViewRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewResult.cs`
- `src/sc.lint.roslyn.abstractions/ToolRuleDescriptor.cs`
- `src/sc.lint.roslyn/operations/IViewOperation.cs`
- `src/sc.lint.roslyn/operations/RunViewOperation.cs`
- `src/sc.lint.roslyn/commands/RegisterViewCommands.cs`
- `src/sc.lint.roslyn/dispatch/IBackendJsonNormalizer.cs`
- `src/sc.lint.roslyn/dispatch/IBackendProcessRunner.cs`
- `src/sc.lint.roslyn/dispatch/DelegatedBackendNormalizationResult.cs`
- `src/sc.lint.roslyn/dispatch/ProcessBackendRunner.cs`
- `src/sc.lint.roslyn/dispatch/BackendJsonNormalizer.cs`
- `src/sc.lint.roslyn/backends/ViewToolsHandler.cs`
- `src/sc.lint.roslyn/backends/ViewRulesHandler.cs`
- `src/sc.lint.roslyn/formatting/ViewRulesHumanOutputFormatter.cs`
- `src/sc.lint.roslyn.demagic.lint/RoslynDeMagicToolModule.cs`
- `tests/sc.lint.roslyn.tests/commands/ViewCommandTests.cs`
- `tests/sc.lint.roslyn.tests/dispatch/ProcessBackendRunnerTests.cs`
- `tests/sc.lint.roslyn.tests/dispatch/BackendJsonNormalizerTests.cs`
- `tests/sc.lint.roslyn.tests/operations/RunViewOperationTests.cs`
- `README.md`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `IViewOperation`, `IBackendJsonNormalizer`,
  `IBackendProcessRunner`
- immutable payload types:
  `ToolDescriptor`, `ToolRuleDescriptor`, `ViewRequest`, `ViewResult`,
  `ViewToolResult`, `ViewRuleResult`, `BackendProcessRequest`,
  `BackendProcessResult`, `DelegatedBackendNormalizationResult<T>`
## Required Work

- implement stable `view` targets needed to use the suite immediately:
  `view tools` and `view rules`
- surface `demagic` rule metadata through `view rules`
- keep standard `.NET` and Roslyn suppression/configuration mechanisms primary
  for suppressing analyzer findings
- do not invent custom attribute types in this sprint unless a concrete need is
  documented first
- implement and test `IBackendProcessRunner` plus the shared JSON-normalization
  path for future delegated process backends
- harden module discovery, descriptor construction, and top-level rendering so
  a second tool can be added without reworking the architecture

## Acceptance Criteria

- `sc-lint-roslyn view tools` and `sc-lint-roslyn view rules` are implemented
- `view` results use the stable top-level envelope
- `demagic` rule metadata is inspectable through the CLI
- delegated process backend execution and JSON normalization are covered by
  `ProcessBackendRunnerTests` and `BackendJsonNormalizerTests`
- the top-level tool-module model is ready for additional tool packages

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- view tools --json`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- view rules --json`
- `git diff --check`
