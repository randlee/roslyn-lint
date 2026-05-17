---
id: A4
title: CLI orchestration planning reset
status: complete
branch: sprint/A4
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A4
target: integration/phase-A
---

# Sprint A4 - CLI Orchestration Planning Reset

## Goal

- Turn A4 into a planning-only sprint for the `roslyn-lint` CLI.
- Replace the ad hoc CLI baseline with one derived from the local
  `creating-ai-clis` skill and the `sc-lint` command-family pattern.
- Leave the current CLI spike explicitly disposable and defer replacement
  implementation to later development work.

## Hard Dependencies

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/phase-A/plan-phase-A.md`
- `docs/phase-A/sprint-A3.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-roslyn-lint-command-surface-and-parser.md`
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-roslyn-lint-command-surface-and-parser.md`
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`
- `docs/adr/INDEX.md`
- `docs/project-plan.md`
- `docs/phase-A/plan-phase-A.md`
- `docs/phase-A/sprint-A4.md`
- `docs/documentation-guidelines.md`

## Future Implementation Inventory

This sprint does not implement the CLI. It defines the exact file inventory
that later CLI development work must follow:

- `src/Roslyn.Lint.Abstractions/`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint.Abstractions/Attributes/` reserved only if later justified
- `src/Roslyn.Lint/Program.cs`
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
- `src/Roslyn.Lint/Contracts/CliEnvelope.cs`
- `src/Roslyn.Lint/Contracts/CliError.cs`
- `src/Roslyn.Lint/Contracts/CliDiagnostic.cs`
- `src/Roslyn.Lint/Contracts/CliErrorKind.cs`
- `src/Roslyn.Lint/Contracts/LintToolRequest.cs`
- `src/Roslyn.Lint/Contracts/LintToolResult.cs`
- `src/Roslyn.Lint/Contracts/LintFinding.cs`
- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint/Contracts/CheckRequest.cs`
- `src/Roslyn.Lint/Contracts/CheckResult.cs`
- `src/Roslyn.Lint/Contracts/ClippyRequest.cs`
- `src/Roslyn.Lint/Contracts/ClippyResult.cs`
- `src/Roslyn.Lint/Contracts/CiRequest.cs`
- `src/Roslyn.Lint/Contracts/CiResult.cs`
- `src/Roslyn.Lint/Contracts/VersionResult.cs`
- `src/Roslyn.Lint/Dispatch/BackendToolDescriptor.cs`
- `src/Roslyn.Lint/Dispatch/IBackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/IBackendProcessRunner.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Operations/ILintToolOperation.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/ICheckOperation.cs`
- `src/Roslyn.Lint/Operations/IClippyOperation.cs`
- `src/Roslyn.Lint/Operations/ICiOperation.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Serialization/RoslynLintJsonContext.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `tests/Roslyn.Lint.Tests/Commands/`
- `tests/Roslyn.Lint.Tests/Contracts/`
- `tests/Roslyn.Lint.Tests/Dispatch/`
- `tests/Roslyn.Lint.Tests/Operations/`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`
- immutable CLI payload types:
  `ToolId`, `ToolDescriptor`, `CliEnvelope<TResult>`, `CliError`, `CliDiagnostic`,
  `BackendToolDescriptor`, `LintToolRequest`, `LintToolResult`,
  `LintFinding`, `ViewRequest`, `ViewResult`, `CheckRequest`, `CheckResult`,
  `ClippyRequest`, `ClippyResult`, `CiRequest`, `CiResult`, `VersionResult`
- enums:
  `CommandFamily`, `LintProfile`, `OutputMode`, `BackendExecutionMode`,
  `CliErrorKind`

## Required Work

- define the stable top-level command families as:
  `lint`, `view`, `check`, `clippy`, `ci`, and `version`
- define the first approved lint target as `demagic`
- define the named lint profiles:
  `fast`, `full`, and `ci`
- define the top-level machine envelope as:
  `ok`, `command`, `data`, `error`, and optional `diagnostics`
- define the stable dotted `command` identifier convention for every command
  family
- define the parser and serializer baseline as
  `System.CommandLine` plus shared `System.Text.Json`
- define how package-owned tools are invoked through `roslyn-lint`, including
  in-process and delegated process execution
- reserve `Roslyn.Lint.Abstractions` as the low-level tool integration package
- prefer standard `.NET` suppression/configuration mechanisms before inventing
  custom suppression attributes
- do not define custom attribute types until a concrete need is documented
- make explicit that the current Spectre-based spike is not the approved
  implementation line
- itemize the exact future files and named types that later CLI implementation
  must create
- keep Phase A as planning-only for the CLI on this branch; do not imply that
  implementation was completed in A4

## Acceptance Criteria

- the CLI baseline is derived from the local `creating-ai-clis` skill rather
  than from the current spike
- the CLI baseline mirrors the `sc-lint` top-level command-family pattern
- the docs define `roslyn-lint` as the umbrella surface for package-owned lint
  tools
- the docs define `ok` / `command` / `data` / `error` / `diagnostics` as the
  stable top-level envelope family
- the docs name `System.CommandLine` as the approved parser baseline
- the docs reserve `Roslyn.Lint.Abstractions` as the one low-level shared
  package in the baseline plan
- the docs keep standard `.NET` suppression mechanisms primary and treat custom
  attributes as suite-specific extensions only
- the docs distinguish `roslyn-lint lint ci` from top-level `roslyn-lint ci`
- the docs itemize the future implementation file inventory and named types
- the repo no longer implies that the current Spectre-based spike is an
  accepted endpoint

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `git diff --check`
