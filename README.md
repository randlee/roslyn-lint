# roslyn-lint

`roslyn-lint` is a repository for Roslyn-based analyzer and CLI tooling aimed
at strongly typed, automation-friendly C# code quality workflows.

## Phase A status

- `Roslyn.DeMagic` is the primary Phase A deliverable and current production-testing target.
- the shipped analyzer rule set for Phase A is `DM001` plus `DM002`
- packaged-consumer validation is in place through the local package-smoke project and
  the expected-diagnostics manifest under `eng/`
- `roslyn-lint` remains a secondary Phase A surface; its contract baseline and current
  commands exist, but analyzer readiness takes precedence over further CLI scope

## Products

- `src/Roslyn.DeMagic`
  Roslyn analyzer package for:
  - `DM001` constant-consolidation enforcement
  - `DM002` forbidden string literal detection
- `src/Roslyn.Lint`
  AI-first CLI surface with a stable JSON envelope, typed errors, and
  MCP-ready DTO reuse, retained here as a supporting suite tool rather than
  the primary Phase A release target

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
`DM002`. Phase A production testing should focus on the analyzer package and
its packaged-consumer validation path first.

## Governing docs

- `docs/roslyn-demagic/requirements.md`
- `docs/roslyn-demagic/architecture.md`
- `docs/roslyn-lint/requirements.md`
- `docs/roslyn-lint/architecture.md`
- `docs/phase-A/plan-phase-A.md`
