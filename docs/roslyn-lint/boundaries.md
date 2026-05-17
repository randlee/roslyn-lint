# roslyn-lint Boundary Inventory

This document captures the CLI-owned boundaries for Phase A.

The current Spectre-based spike does not define these boundaries adequately and
may be removed rather than incrementally repaired.

## CommandSurfaceDefinition

Purpose:

- owns the command and option surface presented by the CLI

Notes:

- every command must support `--json`
- command naming may evolve later, but machine-contract rules may not

## RequestDtoConstruction

Purpose:

- owns transformation from parsed CLI input into explicit request DTOs

Notes:

- this boundary keeps parser concerns out of the operation layer
- request DTOs must remain reusable by a future MCP wrapper
- parser-specific option objects must not leak past this boundary

## OperationLayer

Purpose:

- owns execution of CLI business operations independent of parser and output
  library choices

Notes:

- this is the seam future MCP entrypoints must share
- if current CLI code does not separate this layer, replacement is preferred
- analyzer invocation, workspace traversal, and future repository operations all
  belong here rather than in command handlers

## ResponseDtoConstruction

Purpose:

- owns explicit success and failure payload models returned from operations

Notes:

- success and failure results must stay in one stable contract family
- response DTOs must be transport-neutral business payloads
- the top-level envelope must expose `success`, `operation`, and exactly one of
  `result` or `error`

## JsonSerializationPolicy

Purpose:

- owns the shared `System.Text.Json` settings and serializer context used for
  machine output

Notes:

- CLI and future MCP surfaces must not drift on naming, omission, or enum
  policy
- JSON envelope stability tests should target this boundary directly

## HumanReadableFormatting

Purpose:

- owns presentation-only formatting for interactive or human review use

Notes:

- this boundary is secondary to the machine contract
- no business payload may exist only in this layer

## ErrorContractPolicy

Purpose:

- owns typed error categories, stable machine codes, structured details, and
  suggested actions

Notes:

- `--json` failures must use this same contract family
- prose-only stderr errors are not acceptable as the primary interface
- stable code registries and error kinds should be centralized here

## AdapterBoundary

Purpose:

- owns any future integration seam to external repositories, analyzers,
  workspaces, or services

Notes:

- live and simulator implementations must sit behind the same contract
- if no external integration exists yet, this remains a planned boundary rather
  than a pretext to couple future code directly into the command layer
