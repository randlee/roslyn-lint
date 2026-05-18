---
id: A8
title: View surfaces, boundary metadata, and tool-module hardening
status: complete
branch: sprint/A8
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A8
target: integration/phase-A
---

# Sprint A8 - View Surfaces, Boundary Metadata, And Tool-Module Hardening

## Goal

- Complete the first usable multi-tool foundation for `roslyn-lint`.
- Implement the first stable `view` targets.
- Put the abstractions package to work for suite-specific boundary and tool
  metadata.

## Hard Dependencies

- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/phase-A/sprint-A7.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`

## Exact Targets

- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint.Abstractions/ToolRuleDescriptor.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/RunViewOperation.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Dispatch/IBackendProcessRunner.cs`
- `src/Roslyn.Lint/Dispatch/ProcessBackendRunner.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Backends/ViewToolsHandler.cs`
- `src/Roslyn.Lint/Backends/ViewRulesHandler.cs`
- `src/Roslyn.Lint/Formatting/ViewRulesHumanOutputFormatter.cs`
- `src/Roslyn.DeMagic.Lint/RoslynDeMagicToolModule.cs`
- `tests/Roslyn.Lint.Tests/Commands/ViewCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Dispatch/ProcessBackendRunnerTests.cs`
- `tests/Roslyn.Lint.Tests/Dispatch/BackendJsonNormalizerTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunViewOperationTests.cs`
- `README.md`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `IViewOperation`, `IBackendProcessRunner`
- immutable payload types:
  `ToolDescriptor`, `ToolRuleDescriptor`, `ViewRequest`, `ViewResult`,
  `BackendProcessRequest`, `BackendProcessResult`
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

- `roslyn-lint view tools` and `roslyn-lint view rules` are implemented
- `view` results use the stable top-level envelope
- `demagic` rule metadata is inspectable through the CLI
- delegated process backend execution and JSON normalization are covered by
  `ProcessBackendRunnerTests` and `BackendJsonNormalizerTests`
- the top-level tool-module model is ready for additional tool packages

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj --configuration Release --verbosity normal`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- view tools --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- view rules --json`
- `git diff --check`
