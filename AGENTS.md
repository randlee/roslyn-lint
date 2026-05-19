# AGENTS Instructions for roslyn-lint

## MUST READ

Before participating in ATM team work, read:
- `docs/team-protocol.md`

The messaging protocol in that document is mandatory for all ATM communications.

## Quick Rule

Always follow this sequence for every ATM message:
1. Immediate acknowledgement
2. Do the work
3. Completion summary
4. Immediate completion acknowledgement by receiver

No silent processing.

## Engineering Rule

Simplification is mandatory, not optional.

- Prefer code removal over code addition.
- Do not add workaround paths when a shared-path fix is the real solution.
- Do not accept special-case explanations for simple types or simple contracts.
- If code must be added, explain what old path, fallback, or manual rule it enables us to delete.
- Default to one source of truth, one emission path, and one naming source.
