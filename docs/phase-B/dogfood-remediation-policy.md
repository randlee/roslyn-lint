# B1 Analyzer Dogfood Remediation Policy

## Allowed Dispositions

- `immediate-fix`
- `suppression-with-rationale`
- `config-adjustment`
- `later-phase-remediation`

## Decision Rules

- use `immediate-fix` when the code should be changed during the local adoption
  line
- use `suppression-with-rationale` only when the diagnostic is correct but the
  repository intentionally keeps the pattern
- use `config-adjustment` only when analyzer configuration, not product code,
  is the correct fix surface
- use `later-phase-remediation` only when the work is intentionally deferred
  and linked in `docs/phase-B/dogfood-follow-up-issues.md`
