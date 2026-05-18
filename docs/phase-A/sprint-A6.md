---
id: A6
title: DeMagic backend integration and first usable lint flow
status: complete
branch: sprint/A6
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A6
target: integration/phase-A
---

# Sprint A6 - DeMagic Backend Integration And First Usable Lint Flow

## Goal

- Make `roslyn-lint lint demagic` the first real usable command path.
- Add the dispatch and normalization seams for in-process tool execution.
- Expose rule and finding payloads through the approved CLI contract.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/phase-A/sprint-A5.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-roslyn-lint-command-surface-and-parser.md`
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`

## Exact Targets

- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint.Abstractions/ILintWorkspaceAdapter.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintFinding.cs`
- `src/Roslyn.Lint/Dispatch/BackendToolDescriptor.cs`
- `src/Roslyn.Lint/Dispatch/IBackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/BackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Operations/ILintToolOperation.cs`
- `src/Roslyn.Lint/Operations/RunLintToolOperation.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.DeMagic.Lint/Roslyn.DeMagic.Lint.csproj`
- `src/Roslyn.DeMagic.Lint/DeMagicWorkspaceAdapter.cs`
- `src/Roslyn.DeMagic.Lint/RoslynDeMagicToolModule.cs`
- `src/Roslyn.DeMagic.Lint/RoslynDeMagicLintHandler.cs`
- `src/Roslyn.Lint/Formatting/LintToolHumanOutputFormatter.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Dispatch/BackendToolDispatcherTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunLintToolOperationTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/LintRoslynDeMagicCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Contracts/LintToolSerializationTests.cs`
- `tests/Roslyn.Lint.Tests/TestData/Lint/*`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `ILintWorkspaceAdapter`, `IBackendToolDispatcher`, `ILintToolOperation`
- immutable payload types:
  `BackendToolDescriptor`, `LintToolRequest`, `LintToolResult`, `LintFinding`
- enums:
  `BackendExecutionMode`, `CliErrorKind`

## Required Work

- register `demagic` as the first real tool module
- implement one shared dispatch seam for in-process tool execution
- implement `roslyn-lint lint demagic`
- implement the first `roslyn-lint lint fast` smoke-test path as a documented
  alias to the `demagic` lint flow only
- support a target path option so the first lint flow can run against the
  current repository or fixture-backed test directories
- define the first stable lint payload shape under `data`
- normalize backend success and failure through the top-level envelope
- expose JSON and human output for findings without inventing a second payload
  model
- add fixture-backed CLI tests for success, findings-present, usage-failure,
  and internal-failure paths
- keep the dispatch seam reusable for later delegated process backends
- keep analyzer execution and workspace loading outside the CLI host project

## Acceptance Criteria

- `roslyn-lint lint demagic` works in human and JSON modes
- `roslyn-lint lint demagic --json` emits `lint.demagic`
- `roslyn-lint lint fast --json` works as the documented A6 smoke-test path
  and emits `lint.fast`
- findings are emitted under `data` rather than as family-specific top-level
  fields
- backend failures are normalized into `CliError`
- the dispatch path does not depend on parser-specific types
- the tool module registration path is reusable for future tools
- `RoslynDeMagicToolModule`, `RoslynDeMagicLintHandler`, and the concrete
  `ILintWorkspaceAdapter` implementation live outside `Roslyn.Lint`

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj --configuration Release --verbosity normal`
- `dotnet run --framework net10.0 --project src/Roslyn.Lint/Roslyn.Lint.csproj -- lint demagic --json`
- `dotnet run --framework net10.0 --project src/Roslyn.Lint/Roslyn.Lint.csproj -- lint fast --json`
- `git diff --check`
