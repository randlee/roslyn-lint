# roslyn-lint CLI Contract

This document defines the planned end-to-end contract for the top-level
`roslyn-lint` CLI.

Related ADRs:

- [`../adr/ADR-003-ai-cli-json-contract.md`](../adr/ADR-003-ai-cli-json-contract.md)
- [`../adr/ADR-004-roslyn-lint-command-surface-and-parser.md`](../adr/ADR-004-roslyn-lint-command-surface-and-parser.md)

## Purpose

The top-level `roslyn-lint` CLI is the canonical machine-contract owner for
non-interactive suite commands.

It is not only a launcher. It also owns:

- the stable top-level command families
- canonical machine mode through `--json`
- normalized success and failure envelopes
- backend dispatch to package-owned tools
- stable command identifiers for automation and future MCP parity

## Planned Contract Types

The approved planning baseline explicitly defines these types:

- `CommandFamily`
- `LintProfile`
- `OutputMode`
- `BackendExecutionMode`
- `CliEnvelope<T>`
- `CliError`
- `CliDiagnostic`

## Contract Invariants For Every Non-Interactive Command

Every non-interactive top-level command must preserve the same contract shape:

- success:
  - `ok: true`
  - stable `command`
  - family-owned payload under `data`
  - optional additive `diagnostics`
- failure:
  - `ok: false`
  - stable `command`
  - `CliError` under `error`
  - optional additive `diagnostics`

Commands must not invent family-specific top-level envelope keys such as:

- `findings` at the top level for `lint`
- `report` at the top level for `view`
- `steps` at the top level for `ci`

Those values belong under `data` so the top-level machine contract remains
stable as more tools are added.

## Command Identity Convention

The `command` field is a stable dotted identifier derived from the final CLI
path selected by the caller.

Initial convention:

- `roslyn-lint lint demagic`
  - `lint.demagic`
- `roslyn-lint lint fast`
  - `lint.fast`
- `roslyn-lint lint full`
  - `lint.full`
- `roslyn-lint lint ci`
  - `lint.ci`
- `roslyn-lint view <target>`
  - `view.<target>`
- `roslyn-lint check`
  - `check`
- `roslyn-lint clippy`
  - `clippy`
- `roslyn-lint ci`
  - `ci`
- `roslyn-lint version`
  - `version`

Future product-specific subtargets may extend those patterns, but they must not
replace the approved family names.

## Canonical Success Envelope

Machine-readable success results must use one stable top-level envelope family.

Illustrative shape:

```json
{
  "ok": true,
  "command": "lint.demagic",
  "data": {
    "status": "pass",
    "tool": "demagic",
    "findings": []
  },
  "diagnostics": []
}
```

Required properties:

- top-level success is explicit
- command identity is stable enough for fixtures and automation
- backend payload lives under `data`
- diagnostics are additive and do not replace the business payload

## Canonical Error Envelope

Machine-readable failures must use `CliError` inside the same top-level
contract family.

Illustrative shape:

```json
{
  "ok": false,
  "command": "lint.demagic",
  "error": {
    "kind": "backend_protocol",
    "code": "CLI.BACKEND_PROTOCOL_ERROR",
    "message": "demagic returned unexpected JSON",
    "details": {
      "tool": "demagic"
    },
    "suggested_action": "Re-run with the matching roslyn-lint workspace revision"
  },
  "diagnostics": []
}
```

`CliError` structure:

- `kind`
- `code`
- `message`
- `details`
- `suggested_action`

## Error Kinds

The initial documented top-level error categories are:

- `usage`
- `config`
- `capability`
- `backend_failure`
- `backend_protocol`
- `internal`

These are CLI-level categories. Backend tools may carry more specific analyzer
or domain codes beneath them.

### Error Kind To Stable Code Mapping

| Error kind | Stable code family | Typical meaning |
| --- | --- | --- |
| `usage` | `CLI.USAGE_ERROR` | invalid arguments or unsupported command shape |
| `config` | `CLI.CONFIG_ERROR` | repo config missing, malformed, or contradictory |
| `capability` | `CLI.CAPABILITY_ERROR` | required capability or backend is unavailable |
| `backend_failure` | `CLI.BACKEND_EXEC_FAILURE` | delegated backend failed to execute cleanly or returned a typed failure |
| `backend_protocol` | `CLI.BACKEND_PROTOCOL_ERROR` | delegated backend returned malformed or unexpected machine output |
| `internal` | `CLI.INTERNAL_ERROR` | top-level CLI bug or invariant violation |

## Planned Command-Family Contract Matrix

Every non-interactive command family must be reviewed against the same matrix
before code lands:

| Command family | Stable `command` pattern | Success payload owner | Applicable top-level error kinds |
| --- | --- | --- | --- |
| `lint` | `lint.<tool-or-profile>` | analyzer backend or lint-profile orchestrator | `usage`, `config`, `capability`, `backend_failure`, `backend_protocol`, `internal` |
| `view` | `view.<target>` | view/report backend or adapter layer | `usage`, `config`, `backend_failure`, `backend_protocol`, `internal` |
| `check` | `check` or `check.<target>` | compile or preflight runner | `usage`, `config`, `capability`, `backend_failure`, `backend_protocol`, `internal` |
| `clippy` | `clippy` or `clippy.<target>` | lint-runner backend | `usage`, `config`, `capability`, `backend_failure`, `backend_protocol`, `internal` |
| `ci` | `ci` | top-level orchestration layer | `usage`, `config`, `capability`, `backend_failure`, `backend_protocol`, `internal` |
| `version` | `version` | top-level CLI crate | `usage`, `internal` |

## Backend-To-CLI Normalization

The top-level CLI must normalize backend-native results into the canonical
contract.

### In-Process .NET Backend

When the CLI calls a backend library directly:

- backend success payloads become `CliEnvelope<T>.data`
- typed backend errors are mapped into `CliError`
- backend-specific details may be retained under `details`
- the top-level CLI remains responsible for final `kind` / `code`
  normalization

### Delegated Process Backend

When the CLI dispatches to a package-owned executable:

- the backend must be invoked in machine mode
- the CLI must parse the backend machine payload
- the backend payload is then normalized into the top-level envelope

If the delegated backend:

- exits nonzero with a valid machine-readable failure payload
  - map that payload into `CliError`
- exits nonzero without a valid machine-readable payload
  - emit `CLI.BACKEND_EXEC_FAILURE`
- exits zero with malformed machine-readable output
  - emit `CLI.BACKEND_PROTOCOL_ERROR`

## Profile Policy

The planned profile names are:

- `roslyn-lint lint fast`
- `roslyn-lint lint full`
- `roslyn-lint lint ci`

Profile semantics:

- `fast`
  - low-latency local developer gate
- `full`
  - stronger local pre-push gate
- `ci`
  - lint-only CI-parity profile
- top-level `ci`
  - lint plus tests

The distinction between `roslyn-lint lint ci` and top-level `roslyn-lint ci`
is mandatory.
