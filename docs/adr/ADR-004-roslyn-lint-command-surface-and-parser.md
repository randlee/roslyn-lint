# ADR-004 — roslyn-lint Command Surface, Backend Delegation, And Parser Baseline

| Field | Value |
|---|---|
| ID | ADR-004 |
| Status | **Accepted** |
| Date | 2026-05-17 |
| Deciders | Rand Lee |
| Relates to | REQ-SUITE-CLI-006, REQ-SUITE-CLI-007, REQ-SUITE-CLI-008, REQ-CLI-SURFACE-001, REQ-CLI-SURFACE-006, REQ-CLI-DOTNET-001 |
| Supersedes | — |

---

## Context

`roslyn-lint` is not a one-off CLI for a single analyzer. It is the planned
top-level surface for a multi-tool suite, and the user expects it to track the
same command-family model used by `sc-lint`.

The earlier Phase A CLI spike did not plan around the authoritative local
`creating-ai-clis` skill, did not fix the top-level command families up front,
and preserved an unapproved Spectre-based entrypoint by default.

## Decision Drivers

- `roslyn-lint` must remain the stable top-level executable for the suite
- backend tool packages should be invokable through one umbrella CLI
- the top-level command model should match `sc-lint` wherever the product shape
  is cross-language rather than Rust-specific
- the local `creating-ai-clis` skill recommends `System.CommandLine` for the
  `.NET` parser/binding layer
- backend-specific process flags and payload quirks must remain behind the
  top-level contract boundary

## Decision

`roslyn-lint` adopts the `sc-lint` command-family model as its top-level CLI
shape and uses `System.CommandLine` as the parser baseline for the approved
implementation line.

This means:

- the public top-level command families are:
  `lint`, `view`, `check`, `clippy`, `ci`, and `version`
- `roslyn-lint lint <tool>` is the primary analyzer entry path
- package-owned tools are invoked through `roslyn-lint`, either in-process or
  through delegated process execution
- the first approved lint target is `demagic`
- named profiles are part of the public command model:
  `roslyn-lint lint fast`, `roslyn-lint lint full`, `roslyn-lint lint ci`
- top-level `roslyn-lint ci` remains distinct from `roslyn-lint lint ci`
- `System.CommandLine` is the approved parser and command-registration layer
  for the replacement implementation line
- the current Spectre-based spike is not part of the approved baseline and may
  be removed entirely

## Consequences

### Positive

- the suite gets one explicit umbrella CLI shape before more tools are added
- new package-owned tools can join the surface without inventing new top-level
  command families
- parser choice is no longer implicit or left to accident during implementation
- direct-library and delegated-process backends can coexist behind one contract

### Negative

- current CLI spike code is even less likely to survive the replacement line
- some `sc-lint` subtargets remain product-specific and must be adapted rather
  than copied blindly

## Follow-Up Work

- keep `docs/roslyn-lint/requirements.md`,
  `docs/roslyn-lint/architecture.md`, and
  `docs/roslyn-lint/cli-contract.md` aligned with this decision
- plan future backend tool identifiers individually before they are exposed
- implement the command-registration, dispatch, and normalization seams around
  `System.CommandLine` when CLI development resumes
