# B1 Analyzer Dogfood Remediation Policy

## Allowed Dispositions

- `no-action`
- `immediate-fix`
- `suppression-with-rationale`
- `config-adjustment`
- `later-phase-remediation`

## Decision Rules

- use `no-action` only when an in-scope project emitted no B1 findings and the
  explicit row exists solely to prove coverage
- use `immediate-fix` when the code should be changed during the local adoption
  line
- use `suppression-with-rationale` only when the diagnostic is correct but the
  repository intentionally keeps the pattern
- use `config-adjustment` only when analyzer configuration, not product code,
  is the correct fix surface
- use `later-phase-remediation` only when the work is intentionally deferred
  and linked in `docs/phase-B/dogfood-follow-up-issues.md`

## B1 Classification

| Project | Rule | Classification | Reason |
| --- | --- | --- | --- |
| `src/sc.lint.roslyn.demagic` | `DM001` | `immediate-fix` | The descriptor and analyzer diagnostic ids/categories/messages are genuine shared-constant candidates and should be consolidated in product code, not exempted. |
| `src/sc.lint.roslyn.abstractions` | `none` | `no-action` | No B1 findings were emitted. |
| `src/sc.lint.roslyn.demagic.lint` | `DM002` | `later-phase-remediation` | The inline `demagic` tool id is intentional product vocabulary today, but the repeated literal should be revisited after the CLI and lint-module surfaces are dogfooded together. |
| `src/sc.lint.roslyn` | `DM002` | `immediate-fix` | Repeated command/profile/version literals in production CLI code are real consolidation targets and should move behind one owned source of truth. |
| `tests/sc.lint.roslyn.demagic.tests` | `none` | `no-action` | No B1 findings were emitted. |
| `tests/sc.lint.roslyn.tests` | `DM002` | `later-phase-remediation` | The test suite is asserting public command-surface strings directly. B2 should decide whether those literals move to shared constants or remain literal with explicit policy. |

## Non-Blocking Discovery Note

- B1 is intentionally non-blocking even when a finding is classified
  `immediate-fix`.
- The classification records the correct eventual remedy, not a promise that
  the fix lands inside the same sprint.
