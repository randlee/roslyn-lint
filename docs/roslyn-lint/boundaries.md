# roslyn-lint Boundary Inventory

This document captures the CLI-owned boundaries for the approved planning
baseline.

The current Spectre-based spike does not define these boundaries adequately and
may be removed rather than incrementally repaired.

## CommandSurfaceDefinition

Purpose:

- owns the top-level command families presented by `roslyn-lint`

Notes:

- the family names are fixed:
  `lint`, `view`, `check`, `clippy`, `ci`, `version`
- every non-interactive command must support `--json`
- profile aliases `fast`, `full`, and `ci` belong here
- the current `Program.cs` and `Commands/LintCommand.cs` spike is only a
  temporary holder for this surface and may be replaced wholesale

## CommandIdentityPolicy

Purpose:

- owns the stable dotted `command` identifiers derived from the selected CLI
  path

Notes:

- `roslyn-lint lint demagic` maps to `lint.demagic`
- `roslyn-lint lint fast` maps to `lint.fast`
- top-level `ci` remains `ci`, not `lint.ci`
- machine callers and structured logging must use the same command identity

## RequestDtoConstruction

Purpose:

- owns transformation from parsed CLI input into explicit request DTOs

Notes:

- this boundary keeps parser concerns out of dispatch and backend code
- request DTOs must remain reusable by a future MCP wrapper
- parser-specific option objects must not leak past this boundary
- the preferred request payload families are `LintToolRequest`,
  `ViewRequest`, `CheckRequest`, `ClippyRequest`, and `CiRequest`

## BackendDispatch

Purpose:

- owns backend selection and execution across in-process and delegated package
  implementations

Notes:

- the top-level CLI is the orchestrator for package-owned tools
- backend-specific flags and process details stay here
- the preferred seams are `IBackendToolDispatcher`,
  `IBackendProcessRunner`, and `BackendToolDescriptor`
- backend packages should not call each other directly to satisfy top-level
  command flows

## ResponseNormalization

Purpose:

- owns transformation from backend-native success or failure results into the
  canonical top-level CLI contract

Notes:

- success and failure results must stay in one stable contract family
- backend-native top-level keys must not leak into the public `roslyn-lint`
  envelope
- the preferred payload types are `CliEnvelope<TResult>`, `CliError`,
  `CliDiagnostic`, `LintToolResult`, `LintFinding`, `ViewResult`, and
  `CiResult`

## JsonSerializationPolicy

Purpose:

- owns the shared `System.Text.Json` settings and serializer context used for
  machine output

Notes:

- CLI and future MCP surfaces must not drift on naming, omission, or enum
  policy
- JSON envelope stability tests should target this boundary directly
- the preferred seams are `IJsonEnvelopeWriter` and
  `RoslynLintJsonContext`

## HumanReadableFormatting

Purpose:

- owns presentation-only formatting for interactive or human review use

Notes:

- this boundary is secondary to the machine contract
- no business payload may exist only in this layer
- the preferred presentation seam is `IHumanOutputFormatter<TResponse>`

## ErrorContractPolicy

Purpose:

- owns typed error categories, stable machine codes, structured details, and
  suggested actions

Notes:

- `--json` failures must use this same contract family
- prose-only stderr errors are not acceptable as the primary interface
- stable code registries and error kinds should be centralized here
- the preferred closed vocabulary type is the `CliErrorKind` enum

## ExternalAdapterBoundary

Purpose:

- owns any future integration seam to repositories, services, or external
  tools that sit below the command layer

Notes:

- live and simulator implementations must sit behind the same contract
- if no external integration exists yet, this remains a planned boundary
  rather than permission to couple future code directly into command handlers
