# roslyn-lint Architecture

## 1. Overview

`roslyn-lint` is an AI-first orchestration CLI for a planned family of Roslyn
and repository lint tools.

Its machine contract is the product. Human-readable output is a secondary
presentation layer over the same business payloads.

The current Spectre-based CLI spike is not the approved architecture baseline
and may be deleted.

Relevant ADRs:

- `ADR-001`
- `ADR-003`
- `ADR-004`
- `ADR-005`

## 2. Role

The `roslyn-lint` CLI owns:

- top-level command-family parsing
- repo configuration discovery and loading
- lint-profile resolution
- backend dispatch and orchestration
- top-level success and failure normalization
- canonical machine-readable JSON serialization
- human-readable rendering over normalized payloads

It does not own:

- analyzer business rules
- analyzer configuration semantics that belong to package-specific code
- backend package cross-calls as an orchestration substitute

## 3. Required Layers

The approved implementation must separate these concerns:

- `System.CommandLine` command registration and option binding
- command-family and target resolution
- request DTO construction
- backend dispatch or in-process execution
- response DTO construction
- JSON serialization
- human-readable formatting

Recommended execution shape:

```text
System.CommandLine
  -> command family / target resolution
  -> request DTO
  -> dispatch / operation layer
  -> normalized response DTO
  -> serializer
     -> human formatter
```

Architectural rules:

- request and response DTOs must live outside the entrypoint
- backend dispatch must not be implemented as ad hoc command-family-specific
  shelling-out code
- JSON serialization must not be assembled from formatted text
- human-readable formatting must not be the only tested interface
- if the current CLI spike structure prevents these layers from being cleanly
  separated, deletion and replacement is preferred over incremental patching

## 4. Command-Family Model

The top-level command families are fixed by design:

- `lint`
- `view`
- `check`
- `clippy`
- `ci`
- `version`

Architectural rules:

- the family names above are the stable user-facing umbrella surface
- package-owned tools are selected beneath those families rather than by adding
  new top-level executables
- `demagic` is the first approved lint target
- future lint targets must map to package ownership boundaries explicitly
- `view` remains narrower than `lint` until additional targets are documented
- top-level `ci` remains distinct from `lint ci`

## 5. Backend Dispatch Model

The intended model is:

```text
roslyn-lint
  -> in-process .NET backend, when practical
  -> delegated package-owned executable, when needed
```

The stable contract is the top-level envelope and command surface, not the
dispatch mechanism used behind it.

Dispatch principles:

- backend packages remain self-contained
- `roslyn-lint` decides which backend implementation is used
- backend packages do not call each other directly
- backend replacement must not require changing the top-level CLI contract
- backend-specific flags and payload quirks stay behind the CLI contract
  boundary

## 5.1 Low-Level Shared Package

The planned low-level extensibility package is:

- `src/Roslyn.Lint.Abstractions/`

Its role is intentionally narrow:

- shared tool-module interfaces
- stable tool identifiers and descriptors
- shared enums used by package-owned tools
- suite-specific consumer or source attributes only if a concrete later need is
  documented and standard `.NET` mechanisms do not model it adequately

Architectural rules:

- this package must not depend on `System.CommandLine`
- this package must not own dispatch, normalization, or parser concerns
- standard `.NET` and Roslyn suppression/configuration mechanisms remain the
  default path for analyzer suppression
- custom attributes are reserved only for future needs that are explicitly
  justified by later requirements or ADRs
- `Roslyn.Lint.Core` is deferred until real shared implementation pressure
  exists

## 6. Planned Implementation Units

The approved CLI implementation shape is:

- `src/Roslyn.Lint/Program.cs`
  - thin composition root only
- `src/Roslyn.Lint.Abstractions/`
- `src/Roslyn.Lint.Abstractions/ToolId.cs`
- `src/Roslyn.Lint.Abstractions/ToolDescriptor.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolModule.cs`
- `src/Roslyn.Lint.Abstractions/ILintToolCommandHandler.cs`
- `src/Roslyn.Lint.Abstractions/Attributes/` reserved only if later justified
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
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintToolResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/LintFinding.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ViewRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ViewResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CheckRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CheckResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/ClippyResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiRequest.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/CiResult.cs`
- `src/Roslyn.Lint.Abstractions/Contracts/VersionResult.cs`
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
- `src/Roslyn.Lint/Backends/`

If the current `LintCommand` design prevents this split, it should be removed
and replaced rather than stretched into compliance.

## 6.1 Planned Interfaces, Records/Structs, And Enums

The CLI baseline expects these named types to exist when implementation begins:

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`
- records or immutable payload types:
  `ToolId`, `ToolDescriptor`, `CliEnvelope<TResult>`, `CliError`, `CliDiagnostic`,
  `BackendToolDescriptor`, `LintToolRequest`, `LintToolResult`,
  `LintFinding`, `ViewRequest`, `ViewResult`, `CheckRequest`, `CheckResult`,
  `ClippyRequest`, `ClippyResult`, `CiRequest`, `CiResult`, `VersionResult`
- enums:
  `CommandFamily`, `LintProfile`, `OutputMode`, `BackendExecutionMode`,
  `CliErrorKind`

Type guidance:

- use enums only for closed machine vocabularies
- use immutable records or readonly structs for transport-neutral payloads
- use interfaces for parser-independent dispatch, execution, and rendering
  seams
- do not define custom attribute types until a concrete suite-specific need is
  documented

## 7. Contract Model

The architecture must maintain one stable success and failure contract family.

Minimum contract properties:

- explicit success versus failure discrimination
- stable command identity
- stable field names
- typed errors with machine codes
- structured details for automation
- additive diagnostics that do not replace the business payload

Baseline JSON envelope:

```json
{
  "ok": true,
  "command": "lint.demagic",
  "data": {},
  "diagnostics": []
}
```

```json
{
  "ok": false,
  "command": "lint.demagic",
  "error": {
    "kind": "config",
    "code": "CLI.CONFIG_ERROR",
    "message": "Repository config is missing",
    "details": {
      "path": ".roslyn-lint/config-src.toml"
    },
    "suggested_action": "Create the expected config file or pass an explicit config path"
  },
  "diagnostics": []
}
```

The exact contract matrix and command-identifier rules are owned by
`docs/roslyn-lint/cli-contract.md`.

## 8. MCP-Ready Boundary

The future MCP wrapper must be able to reuse:

- the same request DTOs
- the same response DTOs
- the same dispatch or operation layer
- the same serializer policy

Architectural rules:

- the MCP layer must be thin
- the MCP layer must not rename, flatten, or otherwise reshape business
  payloads
- the CLI must not require stdout parsing for contract reuse

## 9. Mutation And Readback

Whenever the CLI later gains mutating commands, the architecture must pair them
with readback operations.

Architectural rules:

- mutation handlers return explicit machine-readable state
- readback handlers expose resulting state through the same contract family
- tests verify behavior through JSON read-after-write flows

## 10. Adapter And Simulator Boundary

If the CLI integrates with external systems, it must do so through a swappable
interface-based boundary.

Architectural rules:

- live and simulator implementations satisfy the same operation-facing
  contracts
- simulator mode must exercise the same business logic as live mode
- stateful simulator support is part of the architecture, not an optional test
  convenience

## 11. .NET Guidance

Target implementation guidance:

- `System.CommandLine` for parser and option binding
- explicit DTOs
- shared `System.Text.Json` options and serializer context
- thin command-registration layer
- reusable dispatch and operation layer
- one shared JSON envelope model for success and failure
- tests that assert DTO and serialized-field stability directly

Parser-library choice is not open-ended in this baseline. The approved
parser and command-registration layer is `System.CommandLine`, and any future
implementation proposal that deviates from it requires a new ADR.

Boundary ownership detail is defined in `docs/roslyn-lint/boundaries.md`.
