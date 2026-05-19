# ADR-002 — sc.lint.roslyn.demagic AdditionalFiles Configuration Boundary

| Field | Value |
|---|---|
| ID | ADR-002 |
| Status | **Accepted** |
| Date | 2026-05-17 |
| Deciders | Rand Lee |
| Relates to | REQ-DM-CONFIG-001, REQ-DM-CONFIG-004, REQ-DM-CONFIG-007, REQ-DM-CONFIG-008 |
| Supersedes | — |

---

## Context

The PRD defines a configuration model rooted in `.sc-lint-roslyn/` with separate
source and test config files wired through `Directory.Build.props`.

The current analyzer spike does not implement this boundary. Without an
explicit decision, implementation could fall back to source-tree probing,
per-node config parsing, or CLI-owned config behavior that would violate the
project split.

## Decision Drivers

- analyzer behavior must be deterministic inside the Roslyn host
- config selection must follow the compiler-provided analyzer boundary
- source and test projects need separate config payloads without analyzer-side
  heuristics
- missing or invalid config must not crash analysis

## Decision

`sc.lint.roslyn.demagic` configuration is owned inside the analyzer package and enters
through Roslyn `AdditionalFiles` only.

This means:

- the consuming solution provides the applicable config file through
  `Directory.Build.props`
- the analyzer selects config from the `AdditionalFiles` inputs provided by the
  host, not by probing the repository filesystem
- config parsing happens once per compilation start and produces an immutable
  internal config model reused by rule callbacks
- invalid config fails closed for the affected rule and must not crash analysis

## Consequences

### Positive

- config behavior is deterministic and host-aligned
- rule callbacks stay free of ad hoc config parsing
- source and test config selection remains outside analyzer heuristics

### Negative

- consuming solutions must wire `AdditionalFiles` correctly
- configuration bugs will surface as disabled rule behavior until valid config
  is supplied

## Follow-Up Work

- implement shared config model and loader units in `src/sc.lint.roslyn.demagic`
- align tests with missing-config and invalid-config behavior
- document solution wiring in package and repository docs
