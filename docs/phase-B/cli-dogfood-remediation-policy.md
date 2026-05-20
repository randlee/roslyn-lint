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

## B2 Classification

| Finding | Disposition | Why |
| --- | --- | --- |
| `version --json`, `view tools --json`, `check --json`, `clippy --json`, and `ci --json` return stable typed envelopes when the CLI process itself is observed directly | `immediate-fix` | these findings are closed inside B2 by direct dogfooding evidence and do not need follow-up implementation work |
| plain `dotnet run ... --json` prepends build output before the envelope | `contract-clarification` | the machine contract remains valid for the CLI process, but the documentation must explicitly distinguish transport-clean invocation from build-wrapper output |
| repo-root `lint demagic --json` scans the current working directory, which includes package-smoke samples and analyzer testdata in this repository | `contract-clarification` | B2 must document the current target-root rule and record the resulting usability surprise explicitly |
| future repo-owned CLI targeting controls that could exclude `examples/` or `testdata/` by policy | `boundary-guardrail-change` | that follow-up should shape how the later boundary package models owned code roots and non-owned sample/test fixtures |
| concurrent `dotnet run` invocations can race on build outputs and fail with file locks | `later-phase-remediation` | this is a local dogfooding ergonomics issue in the `dotnet run` wrapper path, not a blocker for the shipped CLI contract or Phase B closeout |
