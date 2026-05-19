---
id: A5
title: CLI foundation and abstractions package
status: complete
branch: sprint/A5
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A5
target: integration/phase-A
---

# Sprint A5 - CLI Foundation And Abstractions Package

## Goal

- Replace the current Spectre-based spike with the approved
  `System.CommandLine` foundation.
- Introduce `sc.lint.roslyn.abstractions` as the one low-level shared tool-module
  package.
- Ship a working top-level `sc-lint-roslyn` executable with `version`,
  `view tools`, and parseable command-family skeletons for the remaining
  families.

## Carried Forward From A4

A4 closed with explicit CLI replacement deferrals. A5 owns first delivery of
the undelivered replacement units and must treat the following as mandatory
carry-forward scope from [`sprint-A4.md`](./sprint-A4.md):

- `src/sc.lint.roslyn.abstractions/sc.lint.roslyn.abstractions.csproj`
- `src/sc.lint.roslyn.abstractions/ToolId.cs`
- `src/sc.lint.roslyn.abstractions/ToolDescriptor.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolModule.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolCommandHandler.cs`
- `src/sc.lint.roslyn.abstractions/ILintWorkspaceAdapter.cs`
- `src/sc.lint.roslyn/commands/RegisterLintCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterViewCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCheckCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterClippyCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCiCommand.cs`
- `src/sc.lint.roslyn/commands/RegisterVersionCommand.cs`
- `src/sc.lint.roslyn/commandmodel/CommandFamily.cs`
- `src/sc.lint.roslyn/commandmodel/LintProfile.cs`
- `src/sc.lint.roslyn/commandmodel/OutputMode.cs`
- `src/sc.lint.roslyn/commandmodel/BackendExecutionMode.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliEnvelope.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliError.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliDiagnostic.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliErrorKind.cs`
- `src/sc.lint.roslyn/contracts/VersionResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewResult.cs`
- `src/sc.lint.roslyn/serialization/IJsonEnvelopeWriter.cs`
- `src/sc.lint.roslyn/serialization/RoslynLintJsonContext.cs`
- `src/sc.lint.roslyn/formatting/IHumanOutputFormatter.cs`
- `tests/sc.lint.roslyn.tests/commands/RootCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/LintPlaceholderCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/VersionCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/ViewToolsCommandTests.cs`
- `tests/sc.lint.roslyn.tests/contracts/CliEnvelopeSerializationTests.cs`
- `tests/sc.lint.roslyn.tests/abstractions/ToolDescriptorTests.cs`

## Hard Dependencies

- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/sc-lint-roslyn/boundaries.md`
- `docs/phase-A/sprint-A4.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-sc-lint-roslyn-command-surface-and-parser.md`
- `docs/adr/ADR-005-sc-lint-roslyn-abstractions-package.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `src/sc.lint.roslyn.abstractions/sc.lint.roslyn.abstractions.csproj`
- `src/sc.lint.roslyn.abstractions/ToolId.cs`
- `src/sc.lint.roslyn.abstractions/ToolDescriptor.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolModule.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolCommandHandler.cs`
- `src/sc.lint.roslyn.abstractions/ILintWorkspaceAdapter.cs`
- `src/sc.lint.roslyn.abstractions/Attributes/` reserved only if later justified
- `src/sc.lint.roslyn/Program.cs`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `src/sc.lint.roslyn/commands/RegisterLintCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterViewCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCheckCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterClippyCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCiCommand.cs`
- `src/sc.lint.roslyn/commands/RegisterVersionCommand.cs`
- `src/sc.lint.roslyn/commandmodel/CommandFamily.cs`
- `src/sc.lint.roslyn/commandmodel/LintProfile.cs`
- `src/sc.lint.roslyn/commandmodel/OutputMode.cs`
- `src/sc.lint.roslyn/commandmodel/BackendExecutionMode.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliEnvelope.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliError.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliDiagnostic.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CliErrorKind.cs`
- `src/sc.lint.roslyn/contracts/VersionResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewResult.cs`
- `src/sc.lint.roslyn/serialization/IJsonEnvelopeWriter.cs`
- `src/sc.lint.roslyn/serialization/RoslynLintJsonContext.cs`
- `src/sc.lint.roslyn/formatting/IHumanOutputFormatter.cs`
- `tests/sc.lint.roslyn.tests/commands/RootCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/LintPlaceholderCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/VersionCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/ViewToolsCommandTests.cs`
- `tests/sc.lint.roslyn.tests/contracts/CliEnvelopeSerializationTests.cs`
- `tests/sc.lint.roslyn.tests/abstractions/ToolDescriptorTests.cs`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `ILintWorkspaceAdapter`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`
- immutable payload types:
  `ToolId`, `ToolDescriptor`, `CliEnvelope<TResult>`, `CliError`,
  `CliDiagnostic`, `VersionResult`, `ViewRequest`, `ViewResult`
- enums:
  `CommandFamily`, `LintProfile`, `OutputMode`, `BackendExecutionMode`,
  `CliErrorKind`

## Required Work

- delete the current Spectre command host and replace it with
  `System.CommandLine`
- create `sc.lint.roslyn.abstractions` with the shared types approved in
  `ADR-005`
- keep `sc.lint.roslyn.abstractions` free of parser, dispatch, and CLI-host
  dependencies
- implement the stable top-level command families:
  `lint`, `view`, `check`, `clippy`, `ci`, and `version`
- implement `sc-lint-roslyn version` as a complete working command with stable
  JSON and human output
- implement `sc-lint-roslyn view tools` as a complete working command that lists
  registered tool modules and advertised capabilities
- wire all top-level command families through one shared `--json` path and one
  shared envelope writer
- fail with typed `CliError` payloads for missing subcommands or parse errors
- keep the planned `lint demagic`, `lint fast`, `lint full`, and `lint ci`
  surfaces registered with typed capability deferrals until A6 and A7 land

## Acceptance Criteria

- the current Spectre-based host is removed from `sc.lint.roslyn`
- `sc.lint.roslyn.abstractions` builds and is referenced from `sc.lint.roslyn`
- `sc-lint-roslyn version --json` returns the stable top-level envelope with
  `command: "version"` and is covered by
  `tests/sc.lint.roslyn.tests/commands/VersionCommandTests.cs`
- `sc-lint-roslyn view tools --json` returns the stable top-level envelope with
  `command: "view.tools"` and is covered by
  `tests/sc.lint.roslyn.tests/commands/ViewToolsCommandTests.cs`
- parse errors and missing command paths return JSON `CliError` payloads when
  `--json` is requested and are covered by
  `tests/sc.lint.roslyn.tests/commands/RootCommandTests.cs`
- deferred `lint demagic`, `lint fast`, `lint full`, and `lint ci` command
  paths return typed capability envelopes with `planned_sprint` details and are
  covered by `tests/sc.lint.roslyn.tests/commands/LintPlaceholderCommandTests.cs`
- the command families are registered through `System.CommandLine`
- no low-level shared package depends on the CLI parser or entrypoint

## Deferrals

- `tests/sc.lint.roslyn.tests/commands/CheckPlaceholderCommandTests.cs`
  remains deferred to A7 or A8 while `check` stays a registered placeholder
  surface with explicit typed error responses.
- `tests/sc.lint.roslyn.tests/commands/ClippyPlaceholderCommandTests.cs`
  remains deferred to A7 or A8 while `clippy` stays a registered placeholder
  surface with explicit typed error responses.
- `tests/sc.lint.roslyn.tests/commands/CiPlaceholderCommandTests.cs`
  remains deferred to A7 or A8 while `ci` stays a registered placeholder
  surface with explicit typed error responses.
- rationale: A5 is limited to the first usable CLI foundation and `demagic`
  entry flow preparation. Full placeholder-family test coverage belongs with
  the later sprints that implement the real `check`, `clippy`, and `ci`
  behavior.

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- version --json`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- view tools --json`
- `dotnet run --framework net10.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint demagic --json`
- `git diff --check`
