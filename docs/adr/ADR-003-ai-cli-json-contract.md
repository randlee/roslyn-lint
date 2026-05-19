# ADR-003 — AI-First CLI JSON Contract And MCP-Ready DTO Boundary

| Field | Value |
|---|---|
| ID | ADR-003 |
| Status | **Accepted** |
| Date | 2026-05-17 |
| Deciders | Rand Lee |
| Relates to | REQ-SUITE-CLI-001, REQ-CLI-CONTRACT-001, REQ-CLI-CONTRACT-006, REQ-CLI-ERROR-005, REQ-CLI-MCP-001 |
| Supersedes | — |

---

## Context

The repository already contains a Spectre-based CLI spike, but the approved
design direction comes from the `creating-ai-clis` skill rather than from the
existing implementation.

The user also expects the CLI documentation and future implementation to be
machine-contract first and future-MCP compatible.

## Decision Drivers

- AI and automation are the primary CLI consumers
- the machine contract must be stable before human formatting concerns
- a future MCP wrapper must reuse the same DTOs without reshaping business
  payloads
- the current CLI spike must not force a human-first architecture

## Decision

The `roslyn-lint` CLI uses a stable AI-first JSON contract with transport-
neutral DTOs and a thin command layer.

This means:

- every command supports `--json`
- the baseline JSON envelope includes `ok`, `command`, and exactly one of
  `data` or `error`
- optional top-level `diagnostics` are additive only and must not replace the
  business payload
- error objects include `kind`, `code`, `message`, and optional `details` and
  `suggested_action`
- DTOs live outside the entrypoint and remain reusable by a future MCP wrapper
- human-readable formatting is a secondary rendering layer over the same
  business payloads
- if the current Spectre-based command structure blocks this boundary, it
  should be replaced rather than preserved

## Consequences

### Positive

- the CLI contract is suitable for AI callers and automation
- MCP parity can be added without redesigning payloads
- tests can assert stable JSON rather than parser-specific text output

### Negative

- the current CLI spike is likely to be partially or fully replaced
- command-framework convenience patterns may be rejected if they blur contract
  boundaries

## Follow-Up Work

- keep the CLI requirements and architecture aligned with this envelope and DTO
  boundary
- implement shared contract, serialization, and operation layers when CLI work
  resumes
- ensure CLI tests assert contract stability directly
