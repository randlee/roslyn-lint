---
name: codex-orchestration
version: 0.1.0
description: Orchestrate roslyn-lint sprint work where team-lead coordinates, crl is the sole developer, and quality-mgr enforces the QA gate.
depends_on:
  quality-management-gh: 1.x
  quality-mgr: 0.x
  req-qa: 0.x
  arch-qa: 0.x
  rlint-qa: 0.x
---

# Codex Orchestration

This skill defines the repo-local orchestration workflow for `roslyn-lint`.

## Model

- `team-lead` coordinates sprint sequencing, worktree assignments, and PR flow
- `crl` is the sole developer for Codex-driven implementation work
- `quality-mgr` runs the QA gate after each delivery

## Preconditions

Before starting a sprint:
1. `docs/requirements.md`, `docs/architecture.md`, and `docs/project-plan.md`
   define the sprint or phase review target when the repo is operating under a
   phased plan.
2. A worktree exists for the sprint branch under the repo worktree strategy.
3. The target branch for the sprint is chosen from the current repo plan.
4. The following prompts exist in `.claude/agents/`:
   - `quality-mgr.md`
   - `req-qa.md`
   - `arch-qa.md`
   - `rlint-qa.md`
5. The following QA reporting skill exists in `.claude/skills/`:
   - `quality-management-gh/`
6. `quality-mgr` must also read:
   - `.claude/skills/quality-management-gh/SKILL.md`
7. `sc-compose` is available for rendering the JSON and markdown templates.

## Sprint Flow

1. `team-lead` assigns development to `crl` using `dev-template.xml.j2`.
   Every dev assignment must include the sprint-plan document path as
   `sprint_doc`, and that sprint document is the authoritative source for the
   task. Assignment prose may summarize, but it must not replace or weaken the
   sprint doc.
2. `crl` ACKs, implements, commits, pushes, and reports branch plus SHA.
3. Before QA-1, `crl` performs a self-directed hygiene sweep on the sprint
   branch using the same `review_targets` planned for QA-1 and fixes obvious
   build, test, packaging, and repository-shape issues there. This is a
   developer cleanup step, not a QA surprise.
4. `team-lead` opens or updates the PR.
5. `team-lead` assigns QA to `quality-mgr` using `qa-template.xml.j2`.
   Every QA assignment must include `sprint_doc`, and `quality-mgr` must treat
   that sprint document as the authoritative QA scope source.
6. `quality-mgr` launches the reviewer set:
   - `req-qa`
   - `arch-qa`
   - `rlint-qa`
7. If QA passes and CI is green, merge may proceed.
8. If QA fails, `team-lead` triages the findings across worktrees and
   determines the promoted fix branch.
9. After triage completes, `team-lead` routes concrete fixes back to `crl`
   using `fix-assignment.xml.j2`. Fix assignments must also include
   `sprint_doc`, and the sprint document remains authoritative if the task
   summary omits or compresses details.

## QA Coverage Rule

- `quality-mgr` must extract every deliverable, acceptance criterion, deletion
  target, required validation item, and expected artifact from `sprint_doc`
  before launching `req-qa`
- `req-qa` must independently treat `sprint_doc` as authoritative and must not
  assume the hand-authored deliverables list is exhaustive
- a QA assignment that lists 8 of 12 sprint-doc deliverables is incomplete;
  the 4 omitted items must still be reviewed

## Phase-End Review

For extraction-readiness or phase-close reviews, use `review-template.xml.j2`
to assign a read-only review to `crl`.

## CI

Use standard GitHub CLI:
- `gh pr checks <PR> --watch`
- `gh pr view <PR> --json mergeStateStatus,reviewDecision`

## Assignment Templates

Use the templates in this skill directory:
- `dev-template.xml.j2`
- `fix-assignment.xml.j2`
- `qa-template.xml.j2`
- `review-template.xml.j2`
- `req-qa-assignment.json.j2`
- `arch-qa-assignment.json.j2`
- `rlint-qa-assignment.json.j2`
- `sprint-plan.md.j2`
- reporting templates under `.claude/skills/quality-management-gh/`

## Required Message Sequence

Every ATM task message must follow:
1. ACK
2. Work
3. Completion summary
4. Completion ACK by receiver
