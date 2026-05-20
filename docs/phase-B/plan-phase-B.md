# Phase B Plan

## 1. Goal

Phase B starts local adoption of the shipped `sc.lint.roslyn.demagic` analyzer
and `sc-lint-roslyn` CLI on this repository itself, then uses that experience
to clean up the public package/documentation surface before new product lines
are added.

This is still a planning-driven line. The objective is not new analyzer rules
or boundary-package implementation. The objective is to dogfood the shipped
surfaces, measure the findings they produce on the suite itself, and align the
published package/documentation surface with reality.

## 2. Deliverables

- a formal Phase B sprint plan
- local dogfooding of `sc.lint.roslyn.demagic` on repository source and test
  projects
- local dogfooding of `sc-lint-roslyn` on repository-owned workflows
- a project-by-project findings inventory for the first dogfooding pass
- a remediation classification policy for those findings
- a follow-up issue list for analyzer or CLI behavior that is not yet fully
  predictable or does not match expected consolidation/deduplication behavior
- an explicit decision on whether the first dogfooding pass is non-blocking or
  merge-blocking
- cleaned-up NuGet/package readme content, package metadata, and public
  release-surface documentation for the shipped analyzer and CLI
- corrected repository/project/package URLs for the shipped analyzer and CLI

## 3. Execution Branch

- branch: `integration/phase-B`
- merge target: `develop`

## 4. Hard Dependencies

- `docs/requirements.md`
- `docs/architecture.md`
- `docs/project-plan.md`
- `docs/sc-lint-roslyn-demagic/requirements.md`
- `docs/sc-lint-roslyn-demagic/architecture.md`
- `docs/sc-lint-roslyn/requirements.md`
- `docs/sc-lint-roslyn/architecture.md`
- `docs/phase-A/sprint-A12.md`
- `docs/phase-A/sprint-A13.md`
- `docs/phase-A/production-readiness-checklist.md`
- `eng/sc-lint-roslyn-demagic-package-expected-diagnostics.json`

## 5. Exact Implementation Targets

- `docs/phase-B/sprint-B1.md`
- `docs/phase-B/sprint-B2.md`
- `docs/phase-B/sprint-B3.md`
- `docs/phase-B/dogfood-findings.md`
- `docs/phase-B/cli-dogfood-findings.md`
- `docs/phase-B/dogfood-remediation-policy.md`
- `docs/phase-B/dogfood-follow-up-issues.md`
- `docs/sc-lint-roslyn-demagic/package-usage.md`
- `docs/sc-lint-roslyn/install.md`
- `README.md`
- `docs/releasing.md`
- `src/sc.lint.roslyn.demagic/sc.lint.roslyn.demagic.csproj`
- `src/sc.lint.roslyn/sc.lint.roslyn.csproj`
- `Directory.Build.props`
- `.sc-lint-roslyn/config-src.toml`
- `.sc-lint-roslyn/config-test.toml`
- `tests/sc.lint.roslyn.demagic.tests/`
- `tests/sc.lint.roslyn.tests/`

## 6. Sprint Sequence

| Sprint | Title | Outcome |
| --- | --- | --- |
| B1 | Local analyzer dogfooding | Run `sc.lint.roslyn.demagic` on repository-owned projects locally, systematically inventory consolidation and deduplication findings, and file follow-up issues for anything not fully predictable |
| B2 | Local CLI dogfooding | Run `sc-lint-roslyn` on repository-owned workflows locally, capture contract/usability findings, and file follow-up issues for anything not fully predictable |
| B3 | Published package documentation cleanup | Clean up package readme content, NuGet metadata, release-note presentation, and other public package-surface gaps for the shipped analyzer and CLI |

## 7. Implementation Strategy

- Phase B starts with local adoption, not new rule design
- the first dogfooding pass should prefer analyzer-reference wiring from the
  repo build configuration so local iteration is fast
- CLI dogfooding should use the real `sc-lint-roslyn` command surface and the
  existing JSON contract rather than synthetic documentation-only examples
- source-project and test-project config routing must remain separate under
  `.sc-lint-roslyn/`
- B1 and B2 must treat findings inventory and follow-up issue filing as
  required deliverables, not side effects
- B2 is CLI-only dogfooding and must not absorb boundary-package planning or
  implementation work
- the first dogfooding pass must state the blocking policy explicitly; no
  implicit “warnings for now” or implicit “must fix all findings” language is
  allowed
- unexpected analyzer or CLI behavior must be escalated into explicit
  follow-up issues so predictability gaps are tracked to closure
- B3 must align the public package/documentation surface with the actual
  shipped analyzer and CLI state, including package readme content and feed
  metadata
- B3 must explicitly verify package-reference guidance, CLI install guidance,
  target-framework disclosure, and repository/project URL correctness
- later boundary-package work is intentionally out of Phase B and belongs to
  Phase C

## 8. Acceptance

Phase B planning is established only when:

- `docs/phase-B/plan-phase-B.md`, `docs/phase-B/sprint-B1.md`,
  `docs/phase-B/sprint-B2.md`, and `docs/phase-B/sprint-B3.md` exist
- the plan names the exact dogfooding targets and config surfaces
- the dogfooding sprints require a findings inventory, remediation policy, and
  follow-up issue list
- the dogfooding sprints make the initial blocking policy explicit
- the package-documentation sprint names the exact public package/readme and
  metadata surfaces it must clean up
