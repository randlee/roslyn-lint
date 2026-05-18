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

## Governing docs

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/phase-A/plan-phase-A.md`
