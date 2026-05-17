---
name: sprint-report
description: Generate a sprint status report for the current phase. Default is --table.
---

# Sprint Report Skill

Build fenced JSON and pipe to the Jinja2 template. `mode` controls table vs detailed.

## Usage

```
/sprint-report [--table | --detailed]
```

Default: `--table`

---

## Data Source

**Always use `gh pr list` first** - single call, returns all open PRs with CI and merge state:

```bash
gh pr list --state open --json number,title,headRefName,baseRefName,isDraft,reviewDecision,mergeStateStatus,statusCheckRollup,url
```

This is faster and sufficient for populating `sprint_rows` and `integration_row`. Only drill into individual `gh pr view` or `gh run view` calls if you need failure details or QA status for a specific PR.

If `gh pr list` output is missing information needed to fill the report, note the gap and use targeted follow-up `gh` calls only for the missing fields. Do not silently replace the primary data source with a larger ad hoc workflow.

## Render Command

The template path is relative - run from the repo root or the target worktree root.

```bash
echo '<json>' > /tmp/sprint-report.json
sc-compose render \
  --root .claude/skills/sprint-report \
  --file report.md.j2 \
  --var-file /tmp/sprint-report.json
```

## --table (default)

```json
{
  "mode": "table",
  "sprint_rows": "| AK.1 | ✅ | ✅ | 🏁 | #621 |\n| AK.2 | ✅ | ✅ | 🌀 | #622 |",
  "integration_row": "| **integrate** | | — | 🌀 | — |"
}
```

## --detailed

```json
{
  "mode": "detailed",
  "sprint_rows": "Sprint: AK.1  Contract reconciliation\nPR: #621\nQA: PASS ✓ (iter 3)\nCI: Merged to integrate/phase-AK ✓\n────────────────────────────────────────\nSprint: AK.2  OTel core\nPR: #622\nQA: PASS ✓\nCI: Running (1 pending)",
  "integration_row": "Integration: integrate/phase-AK → develop\nCI: Running — pending AK.4 + AK.5"
}
```

## Icon Reference

| State | DEV | QA | CI |
|-------|-----|----|----|
| Assigned | 📥 | 📥 | |
| In progress | 🌀 | 🌀 | 🌀 |
| Done/Pass | ✅ | ✅ | ✅ |
| Findings | 🚩 | 🚩 | |
| Fixing | 🔨 | | |
| Blocked | | | 🚧 |
| Fail | | | ❌ |
| Merged | | | 🏁 |
| Ready to merge | | | 🚀 |
