---
id: A5
title: CLI foundation and abstractions package
status: complete
branch: sprint/A5
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A5
target: integration/phase-A
---

# Sprint A5 - CLI Foundation And Abstractions Package

## Goal

- Replace the current Spectre-based spike with the approved
  `System.CommandLine` foundation.
- Introduce `Roslyn.Lint.Abstractions` as the one low-level shared tool-module
  package.
- Ship a working top-level `roslyn-lint` executable with `version`,
  `view tools`, and parseable command-family skeletons for the remaining
  families.

## Carried Forward From A4

A4 closed with explicit CLI replacement deferrals. A5 owns first delivery of
the undelivered replacement units and must treat the following as mandatory
carry-forward scope from [`sprint-A4.md`](./sprint-A4.md):

- `src/Roslyn.Lint.Abstractions/Roslyn.Lint.Abstractions.csproj`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint.Abstractions/ILintWorkspaceAdapter.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Commands/RegisterVersionCommand.cs`
- `src/Roslyn.Lint/CommandModel/CommandFamily.cs`
- `src/Roslyn.Lint/CommandModel/LintProfile.cs`
- `src/Roslyn.Lint/CommandModel/OutputMode.cs`
- `src/Roslyn.Lint/CommandModel/BackendExecutionMode.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliError.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliDiagnostic.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint/Contracts/VersionResult.cs`
- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Serialization/RoslynLintJsonContext.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `tests/Roslyn.Lint.Tests/Commands/RootCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/LintPlaceholderCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/VersionCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/ViewToolsCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Contracts/CliEnvelopeSerializationTests.cs`
- `tests/Roslyn.Lint.Tests/Abstractions/ToolDescriptorTests.cs`

## Hard Dependencies

- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/phase-A/sprint-A4.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-roslyn-lint-command-surface-and-parser.md`
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `src/Roslyn.Lint.Abstractions/Roslyn.Lint.Abstractions.csproj`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint.Abstractions/ILintWorkspaceAdapter.cs`
- `src/Roslyn.Lint.Abstractions/Attributes/` reserved only if later justified
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Commands/RegisterVersionCommand.cs`
- `src/Roslyn.Lint/CommandModel/CommandFamily.cs`
- `src/Roslyn.Lint/CommandModel/LintProfile.cs`
- `src/Roslyn.Lint/CommandModel/OutputMode.cs`
- `src/Roslyn.Lint/CommandModel/BackendExecutionMode.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliError.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliDiagnostic.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint/Contracts/VersionResult.cs`
- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Serialization/RoslynLintJsonContext.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `tests/Roslyn.Lint.Tests/Commands/RootCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/LintPlaceholderCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/VersionCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/ViewToolsCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Contracts/CliEnvelopeSerializationTests.cs`
- `tests/Roslyn.Lint.Tests/Abstractions/ToolDescriptorTests.cs`

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
- create `Roslyn.Lint.Abstractions` with the shared types approved in
  `ADR-005`
- keep `Roslyn.Lint.Abstractions` free of parser, dispatch, and CLI-host
  dependencies
- implement the stable top-level command families:
  `lint`, `view`, `check`, `clippy`, `ci`, and `version`
- implement `roslyn-lint version` as a complete working command with stable
  JSON and human output
- implement `roslyn-lint view tools` as a complete working command that lists
  registered tool modules and advertised capabilities
- wire all top-level command families through one shared `--json` path and one
  shared envelope writer
- fail with typed `CliError` payloads for missing subcommands or parse errors
- keep the planned `lint demagic`, `lint fast`, `lint full`, and `lint ci`
  surfaces registered with typed capability deferrals until A6 and A7 land

## Acceptance Criteria

- the current Spectre-based host is removed from `Roslyn.Lint`
- `Roslyn.Lint.Abstractions` builds and is referenced from `Roslyn.Lint`
- `roslyn-lint version --json` returns the stable top-level envelope with
  `command: "version"` and is covered by
  `tests/Roslyn.Lint.Tests/Commands/VersionCommandTests.cs`
- `roslyn-lint view tools --json` returns the stable top-level envelope with
  `command: "view.tools"` and is covered by
  `tests/Roslyn.Lint.Tests/Commands/ViewToolsCommandTests.cs`
- parse errors and missing command paths return JSON `CliError` payloads when
  `--json` is requested and are covered by
  `tests/Roslyn.Lint.Tests/Commands/RootCommandTests.cs`
- deferred `lint demagic`, `lint fast`, `lint full`, and `lint ci` command
  paths return typed capability envelopes with `planned_sprint` details and are
  covered by `tests/Roslyn.Lint.Tests/Commands/LintPlaceholderCommandTests.cs`
- the command families are registered through `System.CommandLine`
- no low-level shared package depends on the CLI parser or entrypoint

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj --configuration Release --verbosity normal`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- version --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- view tools --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- lint demagic --json`
- `git diff --check`
