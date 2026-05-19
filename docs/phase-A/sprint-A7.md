---
id: A7
title: Profiles plus check, clippy, and ci workflows
status: complete
branch: sprint/A7
worktree: /Users/randlee/Documents/github/sc-lint-roslyn-worktrees/sprint/A7
target: integration/phase-A
---

# Sprint A7 - Profiles Plus Check, Clippy, And Ci Workflows

## Goal

- Turn the top-level command families into useful local workflows.
- Implement `lint fast`, `lint full`, `lint ci`, top-level `ci`, `check`, and
  `clippy` using `.NET`-native tools and analyzer gates.

## Hard Dependencies

- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/sc-lint-roslyn/cli-contract.md`
- `docs/phase-A/sprint-A6.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-sc-lint-roslyn-command-surface-and-parser.md`

## Exact Targets

- `src/sc.lint.roslyn/contracts/CheckRequest.cs`
- `src/sc.lint.roslyn/contracts/CheckResult.cs`
- `src/sc.lint.roslyn/contracts/ClippyRequest.cs`
- `src/sc.lint.roslyn/contracts/ClippyResult.cs`
- `src/sc.lint.roslyn/contracts/CiRequest.cs`
- `src/sc.lint.roslyn/contracts/CiResult.cs`
- `src/sc.lint.roslyn/contracts/LintProfileResult.cs`
- `src/sc.lint.roslyn/contracts/WorkflowStepResult.cs`
- `src/sc.lint.roslyn/commandmodel/LintProfile.cs`
- `src/sc.lint.roslyn/commandmodel/LintProfileCatalog.cs`
- `src/sc.lint.roslyn/operations/ICheckOperation.cs`
- `src/sc.lint.roslyn/operations/IClippyOperation.cs`
- `src/sc.lint.roslyn/operations/ICiOperation.cs`
- `src/sc.lint.roslyn/operations/LintProfileRunner.cs`
- `src/sc.lint.roslyn/operations/RepositoryPathResolver.cs`
- `src/sc.lint.roslyn/operations/RunCheckOperation.cs`
- `src/sc.lint.roslyn/operations/RunClippyOperation.cs`
- `src/sc.lint.roslyn/operations/RunCiOperation.cs`
- `src/sc.lint.roslyn/commands/RegisterLintCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCheckCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterClippyCommands.cs`
- `src/sc.lint.roslyn/commands/RegisterCiCommand.cs`
- `src/sc.lint.roslyn/backends/DotnetCommandRunner.cs`
- `src/sc.lint.roslyn/formatting/LintProfileHumanOutputFormatter.cs`
- `src/sc.lint.roslyn/formatting/CheckHumanOutputFormatter.cs`
- `src/sc.lint.roslyn/formatting/ClippyHumanOutputFormatter.cs`
- `src/sc.lint.roslyn/formatting/CiHumanOutputFormatter.cs`
- `tests/sc.lint.roslyn.tests/commands/LintProfileCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/CheckCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/ClippyCommandTests.cs`
- `tests/sc.lint.roslyn.tests/commands/CiCommandTests.cs`
- `tests/sc.lint.roslyn.tests/operations/RunCheckOperationTests.cs`
- `tests/sc.lint.roslyn.tests/operations/RunClippyOperationTests.cs`
- `tests/sc.lint.roslyn.tests/operations/RunCiOperationTests.cs`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `IDotnetCommandRunner`, `ICheckOperation`, `IClippyOperation`, `ICiOperation`
- immutable payload types:
  `CheckRequest`, `CheckResult`, `ClippyRequest`, `ClippyResult`,
  `CiRequest`, `CiResult`, `LintProfileResult`, `WorkflowStepResult`
- enums:
  `LintProfile`, `OutputMode`, `CliErrorKind`

## Required Work

- implement `lint fast`, `lint full`, and `lint ci`
- define the profile membership explicitly in code and docs using one
  authoritative membership table
- implement `check` as the `.NET` build-and-compile gate
- implement `clippy` as the stricter analyzer/build gate using `.NET`-native
  tools rather than Rust-specific behavior
- implement top-level `ci` as lint plus tests
- keep `lint ci` lint-only and distinct from top-level `ci`
- normalize workflow results into the top-level envelope instead of leaking raw
  tool output as the product contract
- return typed capability or dependency failures if required `.NET` tooling is
  unavailable

## Acceptance Criteria

- `sc-lint-roslyn lint fast`, `full`, and `ci` are implemented and documented
- the profile membership table is present in both
  `docs/sc-lint-roslyn/cli-contract.md` and this sprint doc, and the two match
- `sc-lint-roslyn check` is implemented and emits the stable envelope in JSON mode
- `sc-lint-roslyn clippy` is implemented and emits the stable envelope in JSON
  mode
- `sc-lint-roslyn ci` runs lint plus tests and remains distinct from `lint ci`
- profile membership is explicit and test-covered
- raw `dotnet` tool output does not become the public machine contract

## Profile Membership Table

| Profile or command | Required membership in A7 |
| --- | --- |
| `lint fast` | `demagic` |
| `lint full` | `demagic` |
| `lint ci` | `demagic` |
| top-level `ci` | `lint ci` plus `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal` and `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal` |

## Implemented Workflow Definitions

- `check` runs
  `dotnet build sc-lint-roslyn.sln --configuration Release --no-restore`
- `clippy` runs
  `dotnet build sc-lint-roslyn.sln --configuration Release --no-restore -warnaserror`
  followed by
  `dotnet format sc-lint-roslyn.sln --verify-no-changes --no-restore`
- top-level `ci` runs the `lint ci` profile first and then runs
  `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
  followed by
  `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal`

## Required Validation

- `dotnet restore sc-lint-roslyn.sln`
- `dotnet build sc-lint-roslyn.sln --configuration Release`
- `dotnet test tests/sc.lint.roslyn.demagic.tests/sc.lint.roslyn.demagic.tests.csproj --configuration Release --verbosity normal`
- `dotnet test tests/sc.lint.roslyn.tests/sc.lint.roslyn.tests.csproj --configuration Release --verbosity normal`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint fast --json`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint full --json`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- lint ci --json`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- check --json`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- clippy --json`
- `dotnet run --framework net8.0 --project src/sc.lint.roslyn/sc.lint.roslyn.csproj -- ci --json`
- `git diff --check`
