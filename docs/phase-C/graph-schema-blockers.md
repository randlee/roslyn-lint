# Graph Schema Blockers

## Active Blocker

- `C5` graph-export planning is blocked from finalization until the maintainer
  provides the unified cross-language graph schema for Rust and C#.

## Constraint Already Accepted

- until the new unified schema is provided, `sc-lint-roslyn-boundary` should
  remain compatible with the existing `sc-lint-boundary` graph-export contract
  as closely as practical
- once the new schema is provided, it becomes the canonical export contract so
  the same visualization tooling can operate across both languages

## Planning Impact

- `C4` may plan Roslyn graph extraction independently
- `C5` may define only provisional export-plumbing expectations until the
  schema details arrive
- `C5` closeout criteria must reference receipt of maintainer schema details as
  a prerequisite for final plan approval
