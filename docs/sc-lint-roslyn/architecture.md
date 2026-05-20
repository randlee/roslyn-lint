# sc-lint-roslyn Architecture

## 1. Overview

`sc-lint-roslyn` is an AI-first orchestration CLI for a planned family of Roslyn
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

The `sc-lint-roslyn` CLI owns:

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
sc-lint-roslyn
  -> in-process .NET backend, when practical
  -> delegated package-owned executable, when needed
```

The stable contract is the top-level envelope and command surface, not the
dispatch mechanism used behind it.

Dispatch principles:

- backend packages remain self-contained
- `sc-lint-roslyn` decides which backend implementation is used
- backend packages do not call each other directly
- backend replacement must not require changing the top-level CLI contract
- backend-specific flags and payload quirks stay behind the CLI contract
  boundary

## 5.1 Low-Level Shared Package

The planned low-level extensibility package is:

- `src/sc.lint.roslyn.abstractions/`

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
- `sc.lint.roslyn.Core` is deferred until real shared implementation pressure
  exists

## 6. Planned Implementation Units

The approved CLI implementation shape is:

- `src/sc.lint.roslyn/Program.cs`
  - thin composition root only
- `src/sc.lint.roslyn.abstractions/`
- `src/sc.lint.roslyn.abstractions/ToolId.cs`
- `src/sc.lint.roslyn.abstractions/ToolDescriptor.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolModule.cs`
- `src/sc.lint.roslyn.abstractions/ILintToolCommandHandler.cs`
- `src/sc.lint.roslyn.abstractions/Attributes/` reserved only if later justified
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
- `src/sc.lint.roslyn.abstractions/contracts/LintToolRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/LintToolResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/LintFinding.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ViewResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CheckRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CheckResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ClippyRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/ClippyResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CiRequest.cs`
- `src/sc.lint.roslyn.abstractions/contracts/CiResult.cs`
- `src/sc.lint.roslyn.abstractions/contracts/VersionResult.cs`
- `src/sc.lint.roslyn/dispatch/BackendToolDescriptor.cs`
- `src/sc.lint.roslyn/dispatch/IBackendToolDispatcher.cs`
- `src/sc.lint.roslyn/dispatch/IBackendJsonNormalizer.cs`
- `src/sc.lint.roslyn/dispatch/IBackendProcessRunner.cs`
- `src/sc.lint.roslyn/dispatch/DelegatedBackendNormalizationResult.cs`
- `src/sc.lint.roslyn/dispatch/BackendJsonNormalizer.cs`
- `src/sc.lint.roslyn/operations/ILintToolOperation.cs`
- `src/sc.lint.roslyn/operations/IViewOperation.cs`
- `src/sc.lint.roslyn/operations/ICheckOperation.cs`
- `src/sc.lint.roslyn/operations/IClippyOperation.cs`
- `src/sc.lint.roslyn/operations/ICiOperation.cs`
- `src/sc.lint.roslyn/serialization/IJsonEnvelopeWriter.cs`
- `src/sc.lint.roslyn/serialization/RoslynLintJsonContext.cs`
- `src/sc.lint.roslyn/formatting/IHumanOutputFormatter.cs`
- `src/sc.lint.roslyn/backends/`

If the current `LintCommand` design prevents this split, it should be removed
and replaced rather than stretched into compliance.

## 6.1 Planned Interfaces, Records/Structs, And Enums

The CLI baseline expects these named types to exist when implementation begins:

- interfaces:
  `ILintToolModule`, `ILintToolCommandHandler<TRequest, TResponse>`,
  `IBackendToolDispatcher`, `IBackendJsonNormalizer`,
  `IBackendProcessRunner`,
  `ILintToolOperation`, `IViewOperation`, `ICheckOperation`,
  `IClippyOperation`, `ICiOperation`, `IJsonEnvelopeWriter`,
  `IHumanOutputFormatter<TResponse>`
- records or immutable payload types:
  `ToolId`, `ToolDescriptor`, `CliEnvelope<TResult>`, `CliError`, `CliDiagnostic`,
  `BackendToolDescriptor`, `BackendProcessRequest`, `BackendProcessResult`,
  `DelegatedBackendNormalizationResult<T>`, `LintToolRequest`,
  `LintToolResult`, `LintFinding`, `ViewRequest`, `ViewResult`,
  `ViewToolResult`, `ViewRuleResult`, `CheckRequest`, `CheckResult`,
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
      "path": ".sc-lint-roslyn/config-src.toml"
    },
    "suggested_action": "Create the expected config file or pass an explicit config path"
  },
  "diagnostics": []
}
```

The exact contract matrix and command-identifier rules are owned by
`docs/sc-lint-roslyn/cli-contract.md`.

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

## 12. Phase B Dogfooding And Phase C Boundary-Guardrail Direction

Phase B should treat `sc-lint-roslyn` and `sc.lint.roslyn.abstractions` as a
locally dogfooded product surface.

Architectural rules:

- B2 should exercise the CLI through real repo workflows rather than only
  synthetic command tests
- B2 should capture contract drift, usability gaps, and normalization
  surprises as explicit findings and follow-up issues
- future Phase C boundary guard rails for `sc.lint.roslyn.abstractions` should borrow
  the strongest patterns from `sc-lint-boundary`:
  machine-readable boundary records, item-key-level parity, strict planned-gap
  metadata, and automatic warn-to-error escalation for overdue planned gaps
- Phase C planning for boundary enforcement should start from the existing
  abstraction seams and contract DTO inventory rather than inventing a
  separate ad hoc ownership model
- reusable dispatch and operation layer
- one shared JSON envelope model for success and failure
- tests that assert DTO and serialized-field stability directly

Parser-library choice is not open-ended in this baseline. The approved
parser and command-registration layer is `System.CommandLine`, and any future
implementation proposal that deviates from it requires a new ADR.

## 12. Packaging Baseline

`sc.lint.roslyn` is planned as a .NET tool package with a stable executable

Phase B package-surface cleanup must publish the CLI consumer boundary in
`docs/sc-lint-roslyn/install.md`, including install command shape, invocation
example, supported runtime/target-framework expectations, and repository URL.
command name of `sc-lint-roslyn`.

Architectural rules:

- tool packaging must be documented before A13 closes
- package metadata, command name, and publication workflows must describe the
  same shipping model
- GitHub Packages publication may be automated in A13, but the first NuGet.org
  release remains manual and documented
- CI and publish workflows must pack the same `PackAsTool` artifact that local
  validation commands produce
- the release handoff must document the current signing policy, package-source
  setup, exact publish commands, and post-release verification steps for the
  tool package as well as the analyzer package

Boundary ownership detail is defined in `docs/sc-lint-roslyn/boundaries.md`.
