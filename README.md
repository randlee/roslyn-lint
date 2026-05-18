# roslyn-lint

`roslyn-lint` is a repository for Roslyn-based analyzer and CLI tooling aimed
at strongly typed, automation-friendly C# code quality workflows.

## Phase A status

- `Roslyn.DeMagic` is the active implementation line.
- analyzer packaging is validated in CI and prepared for independent release
  as `Roslyn.DeMagic`
- the current `roslyn-lint` CLI is a disposable spike and not the approved
  contract baseline

## Products

- `src/Roslyn.DeMagic`
  Roslyn analyzer package for:
  - `DM001` constant-consolidation enforcement
  - `DM002` forbidden string literal detection
- `src/Roslyn.Lint`
  future AI-first CLI surface with a stable JSON envelope, typed errors, and
  MCP-ready DTO reuse

## Current CLI surfaces

- `roslyn-lint lint demagic`
- `roslyn-lint lint fast|full|ci`
- `roslyn-lint view tools`
- `roslyn-lint view rules`
- `roslyn-lint check`
- `roslyn-lint clippy`
- `roslyn-lint ci`
- `roslyn-lint version`

`view rules` currently surfaces `Roslyn.DeMagic` rule metadata for `DM001` and
`DM002`, while the delegated backend process runner and JSON normalization seam
are in place for future package-owned tool backends.

## Governing docs

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/phase-A/plan-phase-A.md`
