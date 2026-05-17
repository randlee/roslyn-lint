# roslyn-lint Architecture

## 1. Overview

`roslyn-lint` is an AI-first CLI, not a human-first shell utility.

Its machine contract is the product. Human-readable output is a secondary
presentation layer over the same business payloads.

The current Spectre-based CLI spike is not the approved architecture baseline
and may be deleted.

Relevant ADRs:

- `ADR-001`
- `ADR-003`

## 2. Required Layers

The CLI must separate these concerns:

- command parsing and option binding
- request DTO creation
- operation execution
- response DTO creation
- JSON serialization
- human-readable formatting

Recommended execution shape:

```text
CLI parser -> request DTO -> operation layer -> response DTO -> serializer
                                                   -> human formatter
```

Architectural rules:

- request and response DTOs must live outside the entrypoint
- JSON serialization must not be assembled from formatted text
- human-readable formatting must not be the only tested interface
- if the current CLI spike structure prevents these layers from being cleanly
  separated, deletion and replacement is preferred over incremental patching

## 2.1 Planned Implementation Units

The preferred CLI implementation shape is:

- `Program.cs` as a thin composition root only
- `Commands/` for parser-binding and command registration
- `Contracts/Requests/` and `Contracts/Responses/` for transport-neutral DTOs
- `Operations/` for business execution
- `Serialization/` for shared `System.Text.Json` policy and serializer context
- `Formatting/` for human-readable rendering
- `Adapters/` for any external integration seams

If the current `LintCommand` design prevents this split, it should be removed
and replaced rather than stretched into compliance.

## 3. Contract Model

The architecture must maintain one stable success and failure contract family.

Minimum contract properties:

- explicit success versus failure discrimination
- stable command or operation identity
- stable field names
- typed errors with machine codes
- structured details for automation

Baseline JSON envelope:

```json
{
  "success": true,
  "operation": "lint.run",
  "result": {},
  "warnings": []
}
```

```json
{
  "success": false,
  "operation": "lint.run",
  "error": {
    "kind": "validation",
    "code": "CLI.INVALID_PATH",
    "message": "Path does not exist",
    "details": {
      "path": "src/missing"
    },
    "suggested_action": "Provide an existing file or directory path"
  },
  "warnings": []
}
```

The exact command inventory can evolve later, but the contract model must not.

## 4. MCP-Ready Boundary

The future MCP wrapper must be able to reuse:

- the same request DTOs
- the same response DTOs
- the same operation layer
- the same serializer policy

Architectural rules:

- the MCP layer must be thin
- the MCP layer must not rename, flatten, or otherwise reshape business
  payloads
- the CLI must not require stdout parsing for contract reuse

## 5. Mutation and Readback

Whenever the CLI later gains mutating commands, the architecture must pair them
with readback operations.

Architectural rules:

- mutation handlers return explicit machine-readable state
- readback handlers expose resulting state through the same contract family
- tests verify behavior through JSON read-after-write flows

## 6. Adapter and Simulator Boundary

If the CLI integrates with external systems, it must do so through a swappable
interface-based boundary.

Architectural rules:

- the live adapter and simulator implement the same operation-facing contract
- simulator mode must exercise the same business logic as live mode
- stateful simulator support is part of the architecture, not an optional test
  convenience

## 7. .NET Guidance

Target implementation guidance:

- explicit DTOs
- shared `System.Text.Json` options and serializer context where practical
- thin command layer
- reusable operation layer
- one shared JSON envelope model for success and failure
- tests that assert DTO and serialized-field stability directly

Parser-library choice remains a secondary implementation detail. If a chosen
library makes the command surface drift away from this contract model, the
library choice loses.

Boundary ownership detail is defined in `docs/roslyn-lint/boundaries.md`.
