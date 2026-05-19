---
id: A4
title: Packaging and CLI baseline correction
status: closed-with-deferrals
branch: sprint/A4
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A4
target: integration/phase-A
---

# Sprint A4 - Packaging And CLI Baseline Correction

## Goal

- Close the analyzer packaging path and reset the CLI design baseline so it
  matches the repository's AI-first CLI contract instead of the current spike.
- Leave the CLI with an explicit replacement-oriented architecture rather than
  an implied obligation to preserve the current command implementation.

## Hard Dependencies

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/boundaries.md`
- `docs/phase-A/sprint-A3.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Shipped.md`
- `src/sc.lint.roslyn.demagic/AnalyzerReleases.Unshipped.md`
- `src/sc.lint.roslyn/Program.cs`
- `src/sc.lint.roslyn/commands/LintCommand.cs`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `tests/sc.lint.roslyn.tests/commands/LintCommandSettingsTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`

## Deferrals

The CLI replacement units below were planned during A4 but were not delivered
on `sprint/A4`. They are explicitly carried forward into
[`sprint-A5.md`](./sprint-A5.md) and must not be treated as completed A4
deliverables.

- shared abstractions package:
  `src/sc.lint.roslyn.abstractions/sc.lint.roslyn.abstractions.csproj`,
  `ToolId.cs`, `ToolDescriptor.cs`, `ILintToolModule.cs`,
  `ILintToolCommandHandler.cs`
- command registration split:
  `src/sc.lint.roslyn/commands/RegisterLintCommands.cs`,
  `RegisterViewCommands.cs`, `RegisterCheckCommands.cs`,
  `RegisterClippyCommands.cs`, `RegisterCiCommand.cs`,
  `RegisterVersionCommand.cs`
- CLI contract files:
  `src/sc.lint.roslyn.abstractions/contracts/CliEnvelope.cs`,
  `CliError.cs`, `CliDiagnostic.cs`, `CliErrorKind.cs`,
  `VersionResult.cs`, `ViewRequest.cs`, `ViewResult.cs`
- CLI host support seams:
  `src/sc.lint.roslyn/commandmodel/CommandFamily.cs`,
  `LintProfile.cs`, `OutputMode.cs`, `BackendExecutionMode.cs`,
  `src/sc.lint.roslyn/serialization/IJsonEnvelopeWriter.cs`,
  `src/sc.lint.roslyn/serialization/RoslynLintJsonContext.cs`,
  `src/sc.lint.roslyn/formatting/IHumanOutputFormatter.cs`
- A5 validation-oriented tests:
  `tests/sc.lint.roslyn.tests/commands/RootCommandTests.cs`,
  `VersionCommandTests.cs`, `ViewToolsCommandTests.cs`,
  `tests/sc.lint.roslyn.tests/contracts/CliEnvelopeSerializationTests.cs`,
  `tests/sc.lint.roslyn.tests/abstractions/ToolDescriptorTests.cs`

## Important Interfaces, Records/Structs, and Enums

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`
- immutable CLI payload types:
  `ToolId`, `ToolDescriptor`, `CliEnvelope<TResult>`, `CliError`,
  `CliDiagnostic`, `LintToolRequest`, `LintToolResult`, `LintFinding`,
  `ViewRequest`, `ViewResult`, `CheckRequest`, `CheckResult`,
  `ClippyRequest`, `ClippyResult`, `CiRequest`, `CiResult`, `VersionResult`
- enums:
  `CliErrorKind`

## Required Work

- validate analyzer package shape, metadata, and release gating
- ensure repo docs do not treat the current CLI implementation as approved
- align package metadata and release automation with the approved analyzer rule
  set rather than the spike descriptions
- define the CLI requirements and architecture around a stable JSON envelope,
  typed errors, MCP-ready DTOs, and auditable command pairs
- make explicit that `Program.cs` and `LintCommand.cs` are disposable if they
  obstruct the approved CLI layering
- document the exact contract, dispatch, and command-registration types that
  future CLI implementation must create before adding more commands or
  features
- keep the current CLI spike out of external release automation until the
  approved contract types and boundaries exist in code

## Acceptance Criteria

- analyzer packaging is documented and testable
- analyzer package metadata no longer describes rejected spike behavior
- the CLI baseline is aligned to the approved AI-first contract rules
- the CLI baseline names the contract interfaces and immutable payload types
  required before feature expansion
- the repo no longer implies the current CLI spike is the accepted design
- noncompliant CLI spike structure is planned for replacement, not preservation
- analyzer package validation runs in CI independent of CLI release readiness
- the A4 closeout state clearly marks undelivered CLI replacement units as
  deferred to A5 rather than misreporting them as completed on A4

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test sc-lint-roslyn.sln --configuration Release --verbosity normal`
- `dotnet pack src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj --configuration Release --no-build`
- `git diff --check`
