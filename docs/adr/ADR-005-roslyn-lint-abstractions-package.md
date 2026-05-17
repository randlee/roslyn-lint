# ADR-005 — roslyn-lint Abstractions Package And Standard .NET Suppression Preference

| Field | Value |
|---|---|
| ID | ADR-005 |
| Status | **Accepted** |
| Date | 2026-05-17 |
| Deciders | Rand Lee |
| Relates to | REQ-CLI-EXT-001, REQ-CLI-EXT-002, REQ-CLI-EXT-003, REQ-CLI-DOTNET-009 |
| Supersedes | — |

---

## Context

`roslyn-lint` is planned as an umbrella CLI over multiple package-owned lint
tools. Longer term, those tools need one narrow shared contract for tool
registration, metadata, and any suite-specific consumer attributes.

At the same time, the suite should use standard `.NET` and Roslyn suppression
mechanisms wherever they already solve the problem instead of inventing custom
attributes for generic warning suppression.

## Decision Drivers

- keep shared extensibility surface small and transport-neutral
- avoid premature package splitting
- prefer standard `.NET` and Roslyn tooling where it already covers the need
- reserve custom attributes for suite-specific semantics not covered by
  `#pragma`, `SuppressMessage`, or `.editorconfig`

## Decision

The suite reserves one low-level shared package named
`Roslyn.Lint.Abstractions`.

This package is the planned home for:

- shared enums used by tool modules
- shared module and handler interfaces used by package-owned tools
- tool descriptors and stable tool identifiers
- suite-specific consumer or source attributes when standard `.NET` mechanisms
  are insufficient

This means:

- do not split low-level shared types into separate `Annotations` and `Core`
  packages at this stage
- do not introduce `Roslyn.Lint.Core` unless later implementation proves a real
  shared-logic need across multiple tools
- prefer standard suppression/configuration mechanisms first:
  `#pragma warning`, `SuppressMessage`, `.editorconfig`, and normal analyzer
  configuration
- define custom attributes only for suite-specific semantics such as boundary
  declarations, tool metadata, or ownership markers that standard `.NET`
  mechanisms do not model cleanly

## Consequences

### Positive

- one narrow extensibility package gives future tool modules a stable
  integration seam
- custom attribute design stays disciplined instead of becoming a generic
  suppression escape hatch
- the CLI can grow to multiple tools without dragging parser or runtime logic
  into low-level shared contracts

### Negative

- some future shared implementation helpers may need a later extraction if real
  duplication appears
- attribute names for suite-specific semantics remain a later design task

## Follow-Up Work

- keep `docs/roslyn-lint/requirements.md`,
  `docs/roslyn-lint/architecture.md`, and
  `docs/phase-A/sprint-A4.md` aligned with this decision
- introduce `Roslyn.Lint.Core` only if two or more tools prove a real shared
  implementation layer
- document any future custom attribute set explicitly before implementation
