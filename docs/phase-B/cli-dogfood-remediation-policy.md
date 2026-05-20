# B2 CLI Dogfood Remediation Policy

## Allowed Dispositions

- `immediate-fix`
- `contract-clarification`
- `boundary-guardrail-change`
- `later-phase-remediation`

## Decision Rules

- use `contract-clarification` when the CLI contract documentation is
  ambiguous and must be updated in `docs/sc-lint-roslyn/cli-contract.md`
- use `boundary-guardrail-change` only when the finding should shape later
  Phase C boundary ownership or inventory rules
