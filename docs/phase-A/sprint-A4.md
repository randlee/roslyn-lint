---
id: A4
title: Packaging and CLI baseline correction
status: closed-with-deferrals
branch: sprint/A4
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A4
target: integration/phase-A
---

# Sprint A4 - Packaging And CLI Baseline Correction

## Goal

- Close the analyzer packaging path and reset the CLI design baseline so it
  matches the repository's AI-first CLI contract instead of the current spike.
- Leave the CLI with an explicit replacement-oriented architecture rather than
  an implied obligation to preserve the current command implementation.

## Hard Dependencies

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/boundaries.md`
- `docs/phase-A/sprint-A3.md`
- `.claude/skills/creating-ai-clis/`

## Exact Targets

- `src/Roslyn.DeMagic/Roslyn.DeMagic.csproj`
- `src/Roslyn.DeMagic/AnalyzerReleases.Shipped.md`
- `src/Roslyn.DeMagic/AnalyzerReleases.Unshipped.md`
- `src/Roslyn.Lint/Program.cs`
- `src/Roslyn.Lint/Commands/LintCommand.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/LintCommandSettingsTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`

## Deferrals

The CLI replacement units below were planned during A4 but were not delivered
on `sprint/A4`. They are explicitly carried forward into
[`sprint-A5.md`](./sprint-A5.md) and must not be treated as completed A4
deliverables.

- shared abstractions package:
  `src/Roslyn.Lint.Abstractions/Roslyn.Lint.Abstractions.csproj`,
  `ToolId.cs`, `ToolDescriptor.cs`, `ILintToolModule.cs`,
  `ILintToolCommandHandler.cs`
- command registration split:
  `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`,
  `RegisterViewCommands.cs`, `RegisterCheckCommands.cs`,
  `RegisterClippyCommands.cs`, `RegisterCiCommand.cs`,
  `RegisterVersionCommand.cs`
- CLI contract files:
  `src/Roslyn.Lint/Contracts/CliEnvelope.cs`,
  `CliError.cs`, `CliDiagnostic.cs`, `CliErrorKind.cs`,
  `VersionResult.cs`, `ViewRequest.cs`, `ViewResult.cs`
- CLI host support seams:
  `src/Roslyn.Lint/CommandModel/CommandFamily.cs`,
  `LintProfile.cs`, `OutputMode.cs`, `BackendExecutionMode.cs`,
  `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`,
  `src/Roslyn.Lint/Serialization/RoslynLintJsonContext.cs`,
  `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- A5 validation-oriented tests:
  `tests/Roslyn.Lint.Tests/Commands/RootCommandTests.cs`,
  `VersionCommandTests.cs`, `ViewToolsCommandTests.cs`,
  `tests/Roslyn.Lint.Tests/Contracts/CliEnvelopeSerializationTests.cs`,
  `tests/Roslyn.Lint.Tests/Abstractions/ToolDescriptorTests.cs`

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

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release --no-build`
- `git diff --check`
