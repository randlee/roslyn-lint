# Boundary Package Deep Dive

## Goal

Capture the strongest reusable guard rails from `sc-lint-boundary` and the
later `atm-core` boundary-enforcement model so Phase C can plan Roslyn-side
boundary enforcement deliberately.

Primary references:

- `/Users/randlee/Documents/github/sc-lint/README.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-enforcement-model.md`
- `/Users/randlee/Documents/github/sc-lint/docs/sc-lint/boundary-toml-migration.md`
- `/Users/randlee/Documents/github/atm-core/docs/sc-lint/boundary-enforcement-model.md`

## What sc-lint-boundary already does well

- enforces architectural rules with stable rule families rather than one-off
  review comments
- distinguishes different finding classes explicitly:
  visibility violations, external reference violations, external implementation
  violations, cycle findings, and portability findings
- supports machine-readable output in addition to text output
- treats the analyzer itself as a shipped product with documented rule families
- treats boundary policy as repo-owned configuration, not as scattered source
  comments alone

## Highest-value guard rails to borrow

### 1. Machine-readable boundary inventory

`sc-lint` is moving toward canonical TOML-backed boundary records under
`boundaries/`, with Markdown retained as the human explanation layer.

Value for `sc-lint-roslyn`:

- keep Roslyn boundary policy machine-readable
- avoid deriving enforcement from prose-only documents
- allow deterministic linting of contract inventory later

### 2. Item-key-level inventory parity

The strongest enforcement model is not “boundary exists” but “specific boundary
item exists.”

Examples in the Rust model:

- `implementation.module`
- `implementation.type`
- `public.facade` or `public.trait`
- `composition.root`

Value for `sc-lint-roslyn`:

- enforce exact ownership of CLI contract DTOs, abstractions interfaces, and
  composition roots
- track planned versus shipped boundary items without losing granularity

### 3. Planned-gap warn/error escalation

The `sc-lint` / `atm-core` model distinguishes:

- missing and planned in a future sprint: `WARN`
- missing and unplanned: `ERROR`
- missing and overdue: `ERROR`

Value for `sc-lint-roslyn`:

- future boundary enforcement can allow explicitly planned gaps without turning
  warnings into permanent suppressions
- once a planned item is overdue, the guard automatically hardens

### 4. Strict planning metadata

The boundary model requires structured planning metadata:

- stable item key
- owning boundary id
- scheduled sprint
- tracking id
- escalation condition

If metadata is malformed or missing, the model treats the gap as an error.

Value for `sc-lint-roslyn`:

- prevents “we’ll fix this later” prose from becoming a hidden allowlist
- aligns well with the repo’s sprinted planning model

### 5. Strict schema / strict unknown-field posture

The TOML migration plan favors strict rejection of malformed or unknown data.

Value for `sc-lint-roslyn`:

- boundary records should fail closed
- schema drift should be intentional and reviewed

### 6. Dual-loader migration only as a transition

`sc-lint` allows Markdown and TOML during migration, but treats that as a
temporary compatibility mode rather than the long-term source model.

Value for `sc-lint-roslyn`:

- if Roslyn boundary docs begin in Markdown, later machine-readable migration
  should be explicit
- duplicate authoritative definitions across formats must become hard errors

## Recommended Roslyn-side Phase C boundary scope

The highest-value first scope is not “every type in the repo.” It is the
contract and composition boundary around the CLI and abstractions package.

Recommended first inventory families:

- `sc.lint.roslyn.abstractions` public contract types
- CLI composition roots and command-registration roots
- backend dispatch seams
- package-owned tool module seams

Recommended first item classes:

- `implementation.type`
- `implementation.module`
- `public.facade`
- `composition.root`

## Immediate planning consequences for Phase C

- `B1` should dogfood the analyzer and record predictability gaps
- `B2` should dogfood the CLI and record the abstractions/contract findings
  that Phase C must later govern
- `B3` should clean up the public package/documentation surface before the
  boundary package broadens the published product set
- later boundary-enforcement work should begin from machine-readable inventory,
  not from Markdown-only notes
- planned gaps should be warning-eligible only with structured sprint metadata
  and should auto-escalate once overdue

## Recommended follow-on artifacts

These are not required in the planning sprint itself, but Phase C should plan
for them:

- `docs/phase-C/boundary-guardrail-plan.md`
- `boundaries/sc-lint-roslyn/`
- `boundaries/sc-lint-roslyn-abstractions/`
- `boundaries/planning.toml`

## Bottom Line

The most valuable lesson from `sc-lint-boundary` is not a specific Rust rule.
It is the enforcement posture:

- machine-readable boundary inventory
- stable item keys
- planned-gap visibility without indefinite warning debt
- automatic escalation when planned work becomes overdue

That is the right guardrail direction for `sc-lint-roslyn` once Phase C moves
from dogfooding into formal boundary enforcement.
