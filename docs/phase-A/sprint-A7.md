---
id: A7
title: Profiles plus check, clippy, and ci workflows
status: planned
branch: sprint/A7
worktree: /Users/randlee/Documents/github/roslyn-lint-worktrees/sprint/A7
target: integration/phase-A
---

# Sprint A7 - Profiles Plus Check, Clippy, And Ci Workflows

## Goal

- Turn the top-level command families into useful local workflows.
- Implement `lint fast`, `lint full`, `lint ci`, top-level `ci`, `check`, and
  `clippy` using `.NET`-native tools and analyzer gates.

## Hard Dependencies

- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/roslyn-lint/cli-contract.md`
- `docs/phase-A/sprint-A6.md`
- `docs/adr/ADR-003-ai-cli-json-contract.md`
- `docs/adr/ADR-004-roslyn-lint-command-surface-and-parser.md`

## Exact Targets

- `src/Roslyn.Lint.Abstractions/Contracts/CheckRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CheckResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiResult.cs`
- `src/Roslyn.Lint/CommandModel/LintProfile.cs`
- `src/Roslyn.Lint/Operations/ICheckOperation.cs`
- `src/Roslyn.Lint/Operations/IClippyOperation.cs`
- `src/Roslyn.Lint/Operations/ICiOperation.cs`
- `src/Roslyn.Lint/Operations/RunCheckOperation.cs`
- `src/Roslyn.Lint/Operations/RunClippyOperation.cs`
- `src/Roslyn.Lint/Operations/RunCiOperation.cs`
- `src/Roslyn.Lint/Commands/RegisterLintCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCheckCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterClippyCommands.cs`
- `src/Roslyn.Lint/Commands/RegisterCiCommand.cs`
- `src/Roslyn.Lint/Backends/DotnetCommandRunner.cs`
- `tests/Roslyn.Lint.Tests/Commands/LintProfileCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/CheckCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/ClippyCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Commands/CiCommandTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunCheckOperationTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunClippyOperationTests.cs`
- `tests/Roslyn.Lint.Tests/Operations/RunCiOperationTests.cs`

## Important Interfaces, Records/Structs, And Enums

- interfaces:
  `ICheckOperation`, `IClippyOperation`, `ICiOperation`
- immutable payload types:
  `CheckRequest`, `CheckResult`, `ClippyRequest`, `ClippyResult`,
  `CiRequest`, `CiResult`
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

- `roslyn-lint lint fast`, `full`, and `ci` are implemented and documented
- the profile membership table is present in both
  `docs/roslyn-lint/cli-contract.md` and this sprint doc, and the two match
- `roslyn-lint check` is implemented and emits the stable envelope in JSON mode
- `roslyn-lint clippy` is implemented and emits the stable envelope in JSON
  mode
- `roslyn-lint ci` runs lint plus tests and remains distinct from `lint ci`
- profile membership is explicit and test-covered
- raw `dotnet` tool output does not become the public machine contract

## Profile Membership Table

| Profile or command | Required membership in A7 |
| --- | --- |
| `lint fast` | `demagic` plus the explicitly documented low-latency lint set |
| `lint full` | stronger pre-push lint set defined in code and docs |
| `lint ci` | lint-only CI-parity set defined in code and docs |
| top-level `ci` | `lint ci` plus test execution |

## Required Validation

- `dotnet restore roslyn-lint.sln`
- `dotnet build roslyn-lint.sln --configuration Release`
- `dotnet test tests/Roslyn.Lint.Tests/Roslyn.Lint.Tests.csproj --configuration Release --verbosity normal`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- lint fast --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- check --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- clippy --json`
- `dotnet run --project src/Roslyn.Lint/Roslyn.Lint.csproj -- ci --json`
- `git diff --check`
