---
id: A4
title: Packaging and CLI baseline correction
status: complete
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
- `src/Roslyn.Lint.Abstractions/Roslyn.Lint.Abstractions.csproj`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Commands/RegisterVersionCommand.cs`
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
- `src/Roslyn.Lint/Dispatch/IBackendToolDispatcher.cs`
- `src/Roslyn.Lint/Dispatch/IBackendProcessRunner.cs`
- `src/Roslyn.Lint/Dispatch/BackendJsonNormalizer.cs`
- `src/Roslyn.Lint/Operations/ILintToolOperation.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/ICheckOperation.cs`
- `src/Roslyn.Lint/Operations/IClippyOperation.cs`
- `src/Roslyn.Lint/Operations/ICiOperation.cs`
- `src/Roslyn.Lint/Serialization/IJsonEnvelopeWriter.cs`
- `src/Roslyn.Lint/Formatting/IHumanOutputFormatter.cs`
- `src/Roslyn.Lint/Roslyn.Lint.csproj`
- `tests/Roslyn.Lint.Tests/Commands/LintCommandSettingsTests.cs`
- `tests/Roslyn.Lint.Tests/Contracts/CliEnvelopeSerializationTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/LintOperationContractTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/publish.yml`
- `README.md`

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

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test roslyn-lint.sln --configuration Release --verbosity normal`
- `dotnet pack src/Roslyn.DeMagic/Roslyn.DeMagic.csproj --configuration Release --no-build`
- `git diff --check`
