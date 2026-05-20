# B1 Analyzer Dogfood Findings

## Scope

- `src/sc.lint.roslyn.demagic`
- `src/sc.lint.roslyn.abstractions`
- `src/sc.lint.roslyn.demagic.lint`
- `src/sc.lint.roslyn`
- `tests/sc.lint.roslyn.demagic.tests`
- `tests/sc.lint.roslyn.tests`

## Row Contract

| Project | Rule | Location | Finding summary | Expected? | Disposition |
| --- | --- | --- | --- | --- | --- |

## Rules

- every in-scope project must have at least one explicit row or an explicit
  `no findings` note
- every row must map to `DM001`, `DM002`, or a clearly documented analyzer
  behavior gap
