# sc-lint-roslyn

`sc-lint-roslyn` is a repository for Roslyn-based analyzer and CLI tooling aimed
at strongly typed, automation-friendly C# code quality workflows.

## Phase A status

- `sc.lint.roslyn.demagic` is the primary Phase A deliverable and current production-testing target.
- the shipped analyzer rule set for Phase A is `DM001` plus `DM002`
- packaged-consumer validation is in place through the local package-smoke project and
  the expected-diagnostics manifest under `eng/`
- `sc-lint-roslyn` remains a secondary Phase A surface; its contract baseline and current
  commands exist, but analyzer readiness takes precedence over further CLI scope

## Products

- `src/sc.lint.roslyn.demagic`
  Analyzer package for:
  - `DM001` constant-consolidation enforcement
  - `DM002` forbidden string literal detection
- `src/sc.lint.roslyn`
  AI-first CLI surface with a stable JSON envelope, typed errors, and
  MCP-ready DTO reuse, retained here as a supporting suite tool rather than
  the primary Phase A release target

## Current CLI surfaces

- `sc-lint-roslyn lint demagic`
- `sc-lint-roslyn lint fast|full|ci`
- `sc-lint-roslyn view tools`
- `sc-lint-roslyn view rules`
- `sc-lint-roslyn check`
- `sc-lint-roslyn clippy`
- `sc-lint-roslyn ci`
- `sc-lint-roslyn version`

`view rules` currently surfaces `sc.lint.roslyn.demagic` rule metadata for `DM001` and
`DM002`. Phase A production testing should focus on the analyzer package and
its packaged-consumer validation path first.

## Governing docs

- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/phase-A/plan-phase-A.md`
