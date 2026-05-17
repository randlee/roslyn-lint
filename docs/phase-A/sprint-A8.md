---
id: A8
title: View surfaces, boundary metadata, and tool-module hardening
status: planned
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
- `docs/adr/ADR-005-roslyn-lint-abstractions-package.md`

## Exact Targets

- `src/Roslyn.Lint.Abstractions/Attributes/BoundaryDeclarationAttribute.cs`
- `src/Roslyn.Lint.Abstractions/Attributes/LintToolAttribute.cs`
- `src/Roslyn.Lint/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint/Contracts/ViewResult.cs`
- `src/Roslyn.Lint/Operations/IViewOperation.cs`
- `src/Roslyn.Lint/Operations/RunViewOperation.cs`
- `src/Roslyn.Lint/Commands/RegisterViewCommands.cs`
- `src/Roslyn.Lint/Backends/ViewToolsHandler.cs`
- `src/Roslyn.Lint/Backends/ViewRulesHandler.cs`
- `src/Roslyn.Lint/Backends/RoslynDeMagicToolModule.cs`
- `tests/Roslyn.Lint.Tests/Commands/ViewCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunViewOperationTests.cs`
- `tests/Roslyn.Lint.Tests/Abstractions/BoundaryDeclarationAttributeTests.cs`
- `tests/Roslyn.Lint.Tests/Abstractions/LintToolAttributeTests.cs`
- `README.md`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ILintToolModule`, `IViewOperation`
- immutable payload types:
  `ToolDescriptor`, `ViewRequest`, `ViewResult`
- attributes:
  `BoundaryDeclarationAttribute`, `LintToolAttribute`

## Required Work

- implement stable `view` targets needed to use the suite immediately:
  `view tools` and `view rules`
- surface `roslyn-demagic` rule metadata through `view rules`
- use `LintToolAttribute` to mark tool modules with stable IDs and display
  metadata
- use `BoundaryDeclarationAttribute` for suite-specific boundary declarations
  only if the implementation actually needs a source-level marker
- keep standard `.NET` and Roslyn suppression/configuration mechanisms primary
  for suppressing analyzer findings
- harden module discovery, descriptor construction, and top-level rendering so
  a second tool can be added without reworking the architecture

## Acceptance Criteria

- `roslyn-lint view tools` and `roslyn-lint view rules` are implemented
- `view` results use the stable top-level envelope
- `LintToolAttribute` and any boundary attribute use remains suite-specific and
  does not replace normal warning suppression
- `roslyn-demagic` rule metadata is inspectable through the CLI
- the top-level tool-module model is ready for additional tool packages

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj --configuration Release --verbosity normal`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- view tools --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- view rules --json`
- `git diff --check`
