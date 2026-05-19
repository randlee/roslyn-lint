---
id: A6
title: DeMagic backend integration and first usable lint flow
status: complete
branch: sprint/A6
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A6
target: integration/phase-A
---

# Sprint A6 - DeMagic Backend Integration And First Usable Lint Flow

## Goal

- Make `sc-lint-roslyn lint demagic` the first real usable command path.
- Add the dispatch and normalization seams for in-process tool execution.
- Expose rule and finding payloads through the approved CLI contract.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/phase-A/sprint-A5.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-sc-lint-roslyn-command-surface-and-parser.md`
- `docs/adr/ADR-005-sc-lint-roslyn-abstractions-package.md`

## Exact Targets

- `src/sc.lint.roslyn.abstractions/ILintToolModule.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolCommandHandler.cs`
- `src/sc.lint.roslyn.abstractions/ILintWorkspaceAdapter.cs`
- `src/sc.lint.roslyn.abstractions/contracts/LintToolRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/LintToolResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/LintFinding.cs`
- `src/sc.lint.roslyn/dispatch/BackendToolDescriptor.cs`
- `src/sc.lint.roslyn/dispatch/IBackendToolDispatcher.cs`
- `src/sc.lint.roslyn/dispatch/BackendToolDispatcher.cs`
- `src/sc.lint.roslyn/dispatch/BackendJsonNormalizer.cs`
- `src/sc.lint.roslyn/operations/ILintToolOperation.cs`
- `src/sc.lint.roslyn/operations/RunLintToolOperation.cs`
- `src/sc.lint.roslyn/commands/RegisterLintCommands.cs`
- `src/sc.lint.roslyn.demagic.lint/sc.lint.roslyn.demagic.lint.csproj`
- `src/sc.lint.roslyn.demagic.lint/DeMagicWorkspaceAdapter.cs`
- `src/sc.lint.roslyn.demagic.lint/RoslynDeMagicToolModule.cs`
- `src/sc.lint.roslyn.demagic.lint/RoslynDeMagicLintHandler.cs`
- `src/sc.lint.roslyn/formatting/LintToolHumanOutputFormatter.cs`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `tests/sc.lint.roslyn.tests/dispatch/BackendToolDispatcherTests.cs`
- `tests/sc.lint.roslyn.tests/operations/RunLintToolOperationTests.cs`
- `tests/sc.lint.roslyn.tests/commands/LintRoslynDeMagicCommandTests.cs`
- `tests/sc.lint.roslyn.tests/contracts/LintToolSerializationTests.cs`
- `tests/sc.lint.roslyn.tests/testdata/Lint/*`

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
- implement `sc-lint-roslyn lint demagic`
- implement the first `sc-lint-roslyn lint fast` smoke-test path as a documented
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

- `sc-lint-roslyn lint demagic` works in human and JSON modes
- `sc-lint-roslyn lint demagic --json` emits `lint.demagic`
- `sc-lint-roslyn lint fast --json` works as the documented A6 smoke-test path
  and emits `lint.fast`
- findings are emitted under `data` rather than as family-specific top-level
  fields
- backend failures are normalized into `CliError`
- the dispatch path does not depend on parser-specific types
- the tool module registration path is reusable for future tools
- `RoslynDeMagicToolModule`, `RoslynDeMagicLintHandler`, and the concrete
  `ILintWorkspaceAdapter` implementation live outside `sc.lint.roslyn`

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint demagic --json`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint fast --json`
- `git diff --check`
